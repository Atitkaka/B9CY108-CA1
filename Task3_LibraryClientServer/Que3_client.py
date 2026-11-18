# Que3_client.py
# Console client for the online library sign up.
# It asks the user for their details, sends them to the server using TCP
# and shows the registration number that the server sends back.
#
# Start Que3_server.py first, then run:   python Que3_client.py

import socket
import json
import re

SERVER_HOST = "127.0.0.1"
SERVER_PORT = 5050


# ---------- functions to read and check the user input ----------

def get_name():
    while True:
        name = input("Full Name: ").strip()
        if re.fullmatch(r"[A-Za-z][A-Za-z '.-]{1,59}", name):
            return name
        print("  Please enter a valid name (letters only, at least 2 characters).")


def get_address():
    while True:
        address = input("Residential Address: ").strip()
        if len(address) >= 5:
            return address
        print("  Address is too short, please enter your full address.")


def get_contact_number():
    while True:
        number = input("Contact Number: ").strip().replace(" ", "").replace("-", "")
        if re.fullmatch(r"\+?[0-9]{7,15}", number):
            return number
        print("  Please enter a valid phone number (7-15 digits, can start with +).")


def get_email():
    while True:
        email = input("Email Address: ").strip()
        if re.fullmatch(r"[\w.+-]+@[\w-]+(\.[\w-]+)+", email):
            return email
        print("  That is not a valid email address, try again.")


def get_membership():
    while True:
        months = input("Membership duration (6 or 12 months): ").strip()
        if months == "6" or months == "12":
            return int(months)
        print("  Please type 6 or 12.")


def collect_details():
    print("\nPlease enter your details to sign up:\n")
    details = {
        "full_name": get_name(),
        "address": get_address(),
        "contact_number": get_contact_number(),
        "email": get_email(),
        "membership_months": get_membership()
    }
    return details


# ---------- sending to the server ----------

def send_to_server(details):
    # SOCK_STREAM means TCP
    client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client.settimeout(10)
    try:
        client.connect((SERVER_HOST, SERVER_PORT))
        print("\n[+] Connected to server", SERVER_HOST, "on port", SERVER_PORT)

        message = json.dumps(details) + "\n"   # newline marks the end of the message
        client.sendall(message.encode("utf-8"))
        print("[>] Details sent to server:", message.strip())

        # read the reply until the newline
        reply = b""
        while not reply.endswith(b"\n"):
            part = client.recv(1024)
            if not part:
                break
            reply += part

        print("[<] Reply received from server:", reply.decode("utf-8").strip())
        return json.loads(reply.decode("utf-8"))

    except ConnectionRefusedError:
        print("\n[!] Could not connect. Make sure Que3_server.py is running.")
    except socket.timeout:
        print("\n[!] The server did not answer in time.")
    except json.JSONDecodeError:
        print("\n[!] Got an invalid reply from the server.")
    finally:
        client.close()
        print("[x] Connection closed")
    return None


def main():
    print("=========================================")
    print("      ONLINE LIBRARY - MEMBER SIGN UP")
    print("=========================================")

    while True:
        details = collect_details()

        print("\nPlease check your details:")
        print("  Name     :", details["full_name"])
        print("  Address  :", details["address"])
        print("  Phone    :", details["contact_number"])
        print("  Email    :", details["email"])
        print("  Duration :", details["membership_months"], "months")

        confirm = input("\nSend these details? (y/n): ").strip().lower()
        if confirm == "y":
            response = send_to_server(details)
            if response is not None:
                if response.get("status") == "success":
                    print("\n*****************************************")
                    print(" Registration successful!")
                    print(" Your registration number is:", response["registration_number"])
                    print(" Membership valid until:", response["membership_expiry"])
                    print(" Please keep this number for all future use.")
                    print("*****************************************")
                else:
                    print("\nRegistration failed:", response.get("message"))
        else:
            print("Details not sent.")

        again = input("\nRegister another member? (y/n): ").strip().lower()
        if again != "y":
            print("Thank you for using the Online Library. Bye!")
            break


if __name__ == "__main__":
    main()
