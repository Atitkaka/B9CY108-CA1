# Que4.py
# Scrapes the Dublin postal districts from Wikipedia,
# saves them in a JSON file, then moves the data from the JSON file to a CSV file,
# and at the end reads the CSV file and shows everything in the terminal.
#
# Modules used: requests (download page), BeautifulSoup (parse HTML), re, json, csv
# Run:  python Que4.py

import re
import json
import csv
import os

import requests
from bs4 import BeautifulSoup

URL = "https://en.wikipedia.org/wiki/List_of_Dublin_postal_districts"

# save the output files in the same folder as this script
FOLDER = os.path.dirname(os.path.abspath(__file__))
JSON_FILE = os.path.join(FOLDER, "dublin_postal_districts.json")
CSV_FILE = os.path.join(FOLDER, "dublin_postal_districts.csv")


# ---------------- a) scrape the web page ----------------

def download_page(url):
    # Wikipedia asks scripts to send a User-Agent, without it the request can get blocked
    headers = {"User-Agent": "CollegeAssignment-PostalDistrictScraper/1.0 (student project)"}
    response = requests.get(url, headers=headers, timeout=20)
    response.raise_for_status()   # stops the program if the page did not load (404, 403 ...)
    return response.text


def clean_area_name(name):
    # removes words like "parts of", "most of" so only the place name is left
    name = name.strip()
    name = re.sub(r"^(and|or)\s+", "", name)
    name = re.sub(r"^(parts of|part of|most of|central|upper|lower|the)\s+", "", name)
    # "Ballymun east of Ballymun Road" -> "Ballymun"
    name = re.sub(r"\s+(east|west|north|south) of .*$", "", name)
    return name.strip(" .")


def get_areas_from_paragraph(text):
    # Example text:
    # "Dublin 15 includes most of Ashtown, Blanchardstown, Castleknock, ... and Ongar. While the town ..."

    text = re.sub(r"\[.*?\]", "", text)      # remove citation numbers like [4]
    text = re.sub(r"\(.*?\)", "", text)      # remove things in brackets
    text = text.strip().split(".")[0]          # only the first sentence has the areas

    # keep only the part after the "includes ..." words
    for keyword in ["including", "areas such as", "areas around", "includes"]:
        if keyword in text:
            text = text.split(keyword)[-1]
            break

    # D4 sentence goes on with "... and contains the RDS grounds ..." which are not areas
    text = text.split(" and contains")[0]

    areas = []
    for part in text.split(","):
        # some parts have two places joined with "and", e.g. "Henry Street and Mary Street"
        for piece in re.split(r"\s+and\s+", part):
            area = clean_area_name(piece)
            if area != "" and area not in areas:
                areas.append(area)
    return areas


def scrape_postal_districts(html):
    soup = BeautifulSoup(html, "html.parser")
    districts = {}

    # every district has its own heading on the page, e.g. "Dublin 15 (D15)"
    for heading in soup.find_all("h2"):
        match = re.search(r"\((D\d+W?)\)", heading.get_text())
        if match is None:
            continue   # not a district heading (History, References ...)

        code = match.group(1)
        paragraph = heading.find_next("p")   # the areas are in the paragraph after the heading
        if paragraph is None:
            continue

        districts[code] = get_areas_from_paragraph(paragraph.get_text())

    return districts


# ---------------- b) save to JSON ----------------

def save_to_json(data, filename):
    with open(filename, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=4, ensure_ascii=False)
    print("Saved", len(data), "postal districts to", os.path.basename(filename))


# ---------------- c) JSON -> CSV ----------------

def json_to_csv(json_filename, csv_filename):
    # read the data back from the JSON file (not from the web page)
    with open(json_filename, "r", encoding="utf-8") as f:
        data = json.load(f)

    rows_written = 0
    with open(csv_filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(["Postal District", "Area Name"])   # header row
        # one row for each area so the CSV is easy to filter/sort in Excel
        for code, areas in data.items():
            for area in areas:
                writer.writerow([code, area])
                rows_written += 1

    print("Moved data from", os.path.basename(json_filename), "to", os.path.basename(csv_filename),
          "(" + str(rows_written) + " rows)")


# ---------------- d) read CSV and display ----------------

def display_csv(csv_filename):
    print("\n================ DATA FROM CSV FILE ================")
    with open(csv_filename, "r", encoding="utf-8") as f:
        reader = csv.reader(f)
        header = next(reader)
        print(f"{header[0]:<18}{header[1]}")
        print("-" * 52)

        count = 0
        last_code = None
        for row in reader:
            code, area = row[0], row[1]
            if code != last_code and last_code is not None:
                print()   # blank line between districts so it is easier to read
            print(f"{code:<18}{area}")
            last_code = code
            count += 1

    print("-" * 52)
    print("Total rows read from CSV:", count)


def main():
    print("Downloading", URL, "...")
    try:
        html = download_page(URL)
    except requests.exceptions.RequestException as e:
        print("Could not download the page:", e)
        return

    districts = scrape_postal_districts(html)
    if len(districts) == 0:
        print("No postal districts found - the page layout may have changed.")
        return

    print("Found", len(districts), "postal districts")
    save_to_json(districts, JSON_FILE)
    json_to_csv(JSON_FILE, CSV_FILE)
    display_csv(CSV_FILE)


if __name__ == "__main__":
    main()
