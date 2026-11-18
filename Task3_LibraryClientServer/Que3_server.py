# Que3_server.py
# TCP server for the online library sign up.
# It receives the member details from the client, saves them in MongoDB,
# makes a unique registration number and sends it back to the client.
#
# Run this file first:   python Que3_server.py
# To see all members saved in the database:   python Que3_server.py show

import socket
import json
import re
import os
import sys
from datetime import datetime

from pymongo import MongoClient, ReturnDocument
from pymongo.errors import PyMongoError, DuplicateKeyError

HOST = "127.0.0.1"
PORT = 5050

# database connection string (can be changed with an environment variable)
MONGO_URL = os.environ.get("MONGO_URL", "mongodb://localhost:27017/")

mongo_client = MongoClient(MONGO_URL, serverSelectionTimeoutMS=5000)
db = mongo_client["online_library"]
members = db["members"]      # collection where the members are stored
counters = db["counters"]    # used to keep the last registration number


def setup_database():
    # ping checks that MongoDB is actually running
    mongo_client.admin.command("ping")
    # unique indexes so the same registration number / email can't be saved twice
    members.create_index("registration_number", unique=True)
    members.create_index("email", unique=True)
    print("[DB] Connected to MongoDB, database = online_library, collection = members")
    print("[DB] Members already registered:", members.count_documents({}))


def generate_registration_number():
    # $inc increases the counter by 1 in one step, so two clients never get the same number
    counter = counters.find_one_and_update(
        {"_id": "member_id"},
        {"$inc": {"seq": 1}},
        upsert=True,
        return_document=ReturnDocument.AFTER
    )
    year = datetime.now().year
    return "LIB" + str(year) + str(counter["seq"]).zfill(4)   # e.g. LIB20260001


def add_months(start_date, months):
    # small helper to work out when the membership ends
    month = start_date.month - 1 + months
    year = start_date.year + month // 12
    month = month % 12 + 1
    day = min(start_date.day, 28)
    return start_date.replace(year=year, month=month, day=day)


def validate_data(data):
    # the server checks the data again, we should never just trust the client
    required = ["full_name", "address", "contact_number", "email", "membership_months"]
    for field in required:
        if field not in data or str(data[field]).strip() == "":
            return "Missing field: " + field

    if not re.fullmatch(r"[A-Za-z][A-Za-z '.-]{1,59}", data["full_name"]):
        return "Invalid name"
    if len(data["address"]) < 5:
        return "Address is too short"
    if not re.fullmatch(r"\+?[0-9]{7,15}", data["contact_number"]):
        return "Invalid contact number"
    if not re.fullmatch(r"[\w.+-]+@[\w-]+(\.[\w-]+)+", data["email"]):
        return "Invalid email address"
    if data["membership_months"] not in [6, 12]:
        return "Membership must be 6 or 12 months"
    return None


def receive_message(conn):
    # keep reading until we get the newline at the end of the message
    # (recv might not get all the data in one go)
    data = b""
    while not data.endswith(b"\n"):
        part = conn.recv(1024)
        if not part:
            break   # client closed the connection
        data += part
    return data.decode("utf-8").strip()


def send_message(conn, message_dict):
    text = json.dumps(message_dict) + "\n"
    conn.sendall(text.encode("utf-8"))


def handle_client(conn, addr):
    print("\n[+] Connection established with client", addr)
    try:
        raw = receive_message(conn)
        if raw == "":
            print("[-] No data received from", addr)
            return

        print("[<] Data received:", raw)
        data = json.loads(raw)

        error = validate_data(data)
        if error is not None:
            print("[!] Validation failed:", error)
            send_message(conn, {"status": "error", "message": error})
            return

        # check if this email already has a membership
        existing = members.find_one({"email": data["email"].lower()})
        if existing is not None:
            msg = "This email is already registered (Registration No: " + existing["registration_number"] + ")"
            print("[!]", msg)
            send_message(conn, {"status": "error", "message": msg})
            return

        reg_number = generate_registration_number()
        today = datetime.now()

        member = {
            "registration_number": reg_number,
            "full_name": data["full_name"],
            "address": data["address"],
            "contact_number": data["contact_number"],
            "email": data["email"].lower(),
            "membership_months": data["membership_months"],
            "registered_on": today,
            "membership_expiry": add_months(today, data["membership_months"])
        }
        members.insert_one(member)
        print("[DB] Member saved in MongoDB with registration number", reg_number)

        send_message(conn, {
            "status": "success",
            "registration_number": reg_number,
            "membership_expiry": member["membership_expiry"].strftime("%d-%m-%Y")
        })
        print("[>] Registration number sent back to client")

    except json.JSONDecodeError:
        print("[!] Data from client was not valid JSON")
        send_message(conn, {"status": "error", "message": "Bad data format"})
    except DuplicateKeyError:
        send_message(conn, {"status": "error", "message": "Member already exists"})
    except PyMongoError as e:
        print("[!] Database error:", e)
        send_message(conn, {"status": "error", "message": "Server database error, try again later"})
    except (ConnectionResetError, ConnectionAbortedError):
        print("[!] Client disconnected suddenly")
    finally:
        conn.close()
        print("[x] Connection with", addr, "closed")


def show_members():
    # prints everything saved in the members collection
    print("Members in MongoDB (online_library.members):\n")
    count = 0
    for m in members.find().sort("registration_number", 1):
        count += 1
        print(m["registration_number"], "|", m["full_name"], "|", m["email"], "|",
              m["contact_number"], "|", m["address"], "|", m["membership_months"], "months | expires",
              m["membership_expiry"].strftime("%d-%m-%Y"))
    print("\nTotal members:", count)


def start_server():
    try:
        setup_database()
    except PyMongoError as e:
        print("Could not connect to MongoDB. Is the MongoDB service running?")
        print("Error:", e)
        return

    # AF_INET = IPv4, SOCK_STREAM = TCP (connection oriented)
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind((HOST, PORT))
    server.listen(5)
    # timeout so that Ctrl+C works on Windows while waiting in accept()
    server.settimeout(1.0)

    print("[*] Library server is listening on", HOST, "port", PORT)
    print("[*] Press Ctrl+C to stop the server")

    try:
        while True:
            try:
                conn, addr = server.accept()
            except socket.timeout:
                continue
            conn.settimeout(None)
            handle_client(conn, addr)
    except KeyboardInterrupt:
        print("\n[*] Server stopped")
    finally:
        server.close()
        mongo_client.close()


if __name__ == "__main__":
    if len(sys.argv) > 1 and sys.argv[1] == "show":
        show_members()
    else:
        start_server()
