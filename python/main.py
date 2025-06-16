import base64
import base64
import hashlib
import json
import logging
import ollama
import os
import psutil
import re
import secrets
import sqlite3
import subprocess
import threading
import time
from threading import Lock

# from mcp.server.fastmcp import FastMCP
# import requests

# Set up logging
logging.basicConfig(level=logging.INFO,
                    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
logger = logging.getLogger("AIModule")


# this is code for mcp server

# mcp = FastMCP("MCP_SERVER")
# @mcp.tool()
# def connect_ai(prompt : str) -> str:
#     """Send a prompt to DeepSeek R1 via Ollama and return the response.
#     Optionally include a system prompt."""
#
#     ai_LLM = AI()
#     ai_LLM.main(prompt)
#     return "Connected to AI server"


def concise_text(text):
    # // this function will remove all the special characters from the text

    # First remove the <think> tags and their content
    cleaned_txt = re.sub(r'<think>.*?</think>', '', txt)

    # Extract text between double asterisks, replacing "**text**" with "text"
    cleaned_txt = re.sub(r'\*\*(.*?)\*\*', r'\1', cleaned_txt)

    # If you have triple asterisks, you can handle them as well
    cleaned_txt = re.sub(r'\*\*\*(.*?)\*\*\*', r'\1', cleaned_txt)

    # Remove any remaining special characters
    cleaned_txt = re.sub(r'[^\w\s]', '', cleaned_txt)

    return cleaned_txt


class AI:
    def __init__(self):
        self._client = None
        self._opt = None
        self._ollama_server = None
        self._messages_ollama = []
        self._system_txt = "You are a helpful assistant."
        self._response_str = ""
        self._lock = Lock()  # Add thread safety
        self._server_started = False

        # self.mcp = FastMCP("AI_SERVER")

    def set_parameters(self, dict1: {}):
        self._opt = dict1

    @staticmethod
    def con(stop_thinking=False):
        try:
            startupinfo = subprocess.STARTUPINFO()
            startupinfo.dwFlags |= subprocess.STARTF_USESHOWWINDOW

            # Start the Ollama server
            ollama_server = subprocess.Popen(
                ["ollama", "serve"],
                stdout=subprocess.DEVNULL,
                stderr=subprocess.DEVNULL,
                startupinfo=startupinfo
            )

            # Wait for server to start up
            time.sleep(1)

            # Configure options
            if stop_thinking:
                opt = {
                    "temperature": 0.5,
                    "keep_alive": '-1',
                    "top_p": 0.5,
                    "num_predict": 150
                }
            else:
                # opt = {
                #     "temperature": 0.5,
                #     "keep_alive": '-1',
                #     "top_p": 0.5,
                #     "num_predict": 800,
                # }
                # this is more good in quality and long
                opt = {
                    "temperature": 0.1,
                    "keep_alive": '-1',
                    "top_p": 0.1,
                    "num_predict": 40000
                }

            # Create client
            client = ollama.Client()

            # Ping to verify connection
            try:
                client.list()
                logger.info("Ollama server connected successfully")
            except Exception as e:
                logger.error(f"Failed to connect to Ollama server: {str(e)}")
                raise ConnectionError(f"Could not connect to Ollama server: {str(e)}")

            return client, opt, ollama_server

        except Exception as e:
            logger.error(f"Error in con method: {str(e)}")
            raise Exception(f"Failed to initialize Ollama: {str(e)}")

    def set_system_txt(self, system_txt):
        with self._lock:
            self._system_txt = system_txt
            logger.info("System text updated")

    def __del__(self):
        try:
            if self._ollama_server:
                self._ollama_server.kill()
                logger.info("Ollama server terminated")
        except Exception as e:
            logger.error(f"Error shutting down Ollama server: {str(e)}")

    def user_message(self, content):
        with self._lock:
            self._messages_ollama.clear()
            self._messages_ollama.append({'role': 'system', 'content': self._system_txt})
            self._messages_ollama.append({'role': 'user', 'content': content})

    def main(self, message):
        """this tool got called when ever the user ask the prompt because it contains the deep seek r1 core code that"""
        try:
            # Reset response string
            self._response_str = ""

            # Set up the user message
            self.user_message(message)

            # Initialize the client if needed
            if not self._client or not self._ollama_server:
                self._client, self._opt, self._ollama_server = self.con()

            # Get response from model
            response = self._client.chat(
                model='deepseek-r1:7b',
                messages=self._messages_ollama,
                options=self._opt,
                stream=True
            )

            # response = requests.post(
            #     "http://localhost:11434/api/generate",
            #     json={
            #         model: "deepseek-r1:7b",
            #         "messages": self._messages_ollama,
            #         "options": self._opt,
            #         "stream": True
            #     },
            #     timeout=120,
            # )

            # Process response chunks
            for chunk in response:
                if chunk and 'message' in chunk and 'content' in chunk['message']:
                    self._response_str += chunk['message']['content']

            return self._response_str

        except Exception as e:
            logger.error(f"Error in main method: {str(e)}")
            return f"Error generating response: {str(e)}"

    def kill(self):
        try:
            if self._ollama_server:
                logger.info(f"Terminating ollama server with PID: {self._ollama_server.pid}")
                self._ollama_server.terminate()
                self._ollama_server.wait(timeout=5)
                logger.info("Ollama server terminated")
        except Exception as e:
            logger.error(f"Error shutting down Ollama server: {str(e)}")
        finally:
            if os.name == 'nt':
                try:
                    # Try multiple possible process names
                    for process_name in ["ollama.exe", "ollama_llama_server.exe", "ollama_server.exe"]:
                        logger.info(f"Attempting to kill process: {process_name}")
                        for proc in psutil.process_iter(['pid', 'name']):
                            if proc.info['name'] == process_name:
                                logger.info(f"Found and killing {process_name} with PID: {proc.info['pid']}")
                                proc.kill()

                    # Show all running processes with "ollama" in the name for debugging
                    ollama_processes = [p.info for p in psutil.process_iter(['pid', 'name'])
                                        if 'ollama' in p.info['name'].lower()]
                    if ollama_processes:
                        logger.info(f"Found ollama processes: {ollama_processes}")
                        for proc_info in ollama_processes:
                            subprocess.run(["taskkill", "/F", "/PID", str(proc_info['pid'])], capture_output=True)
                except Exception as e:
                    logger.error(f"Error during process cleanup: {str(e)}")


class LoginAI:
    _current_user = None

    def __init__(self, db_path="users.db"):
        self.db_path = os.path.abspath("../../../python/users.db")
        self.userpresent = False  # this flag for check that user is already exist or not
        self.current_user = LoginAI._current_user

    def create_table(self):
        logger.error(os.path.abspath(self.db_path))
        with sqlite3.connect(self.db_path) as conn:
            conn.execute(
                """
                CREATE TABLE IF NOT EXISTS users(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT,
                hashed_password TEXT,
                salt TEXT
                )
                """
            )
            conn.execute(
                """
                CREATE TABLE IF NOT EXISTS headings (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                user_id INTEGER,
                heading TEXT,
                FOREIGN KEY (user_id) REFERENCES users(id)
                )
                """
            )
            conn.execute(
                """
                CREATE TABLE IF NOT EXISTS chat_history (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                heading_id INTEGER,
                prompt TEXT,
                response TEXT,
                FOREIGN KEY (heading_id) REFERENCES headings(id)
                )
                """
            )
            conn.commit()
            logger.error(os.path.abspath(self.db_path))

    def _generate_salt(self):
        # this method will called when we have to insert user into data base
        return secrets.token_bytes(16)

    def _hash_password(self, password, salt):
        return hashlib.pbkdf2_hmac("sha256", password.encode(), salt, 100000)

    def create_account(self, username, password):
        with sqlite3.connect(self.db_path) as conn:
            cursor = conn.cursor()
            cursor.execute(
                """
                SELECT 1 FROM users WHERE username = ?
                """, (username,)
            )

            if cursor.fetchone():
                self.userpresent = True
                return False  # user is already present

            salt = self._generate_salt()
            hashed_password = self._hash_password(password, salt)

            salt_b64 = base64.b64encode(salt)
            hashed_password_b64 = base64.b64encode(hashed_password)

            cursor.execute(
                """
                INSERT INTO users(username, hashed_password, salt) VALUES(?, ?, ?)
                """, (username, hashed_password_b64, salt_b64)
            )

            conn.commit()
            LoginAI._current_user = username
            return True  # account has been made

    def login(self, username, password):
        with sqlite3.connect(self.db_path) as conn:
            cursor = conn.cursor()
            cursor.execute(
                """
                SELECT salt, hashed_password FROM users WHERE username = ?
                """, (username,)
            )
            row = cursor.fetchone()
            if row:  # user is present in the db
                salt, hashed_password = row
                salt = base64.b64decode(salt)
                hashed_password = base64.b64decode(hashed_password)
                if self._hash_password(password, salt) == hashed_password:
                    self.current_user = username
                    LoginAI._current_user = username
                    return True
                else:
                    return False

            else:
                return False

    @classmethod
    def get_username(cls):
        return cls._current_user


class HeadingAI:
    def __init__(self, content):
        self.content = content
        self.heading = None

        # content must not have think part

    def create_heading(self):
        if self.heading is None:
            return None
        else:
            title_generator = pipeline("summarization", model="t5-small")
            summary = title_generator(self.heading, max_length=10, min_length=10, do_sample=False)
            return summary[0]['summary_text'].strip()


class MakeRememberMe:

    def __init__(self):
        self._username = None
        self._password = None

        self._path = "data.json"

    def set_data(self, username, password):
        self._username = username
        self._password = password

    def save_data(self):
        # here username and password are encrypted
        data = {"username": self._username, "password": self._password}

        with open(self._path, 'w') as file:
            json.dump(data, file, indent=4)

    def load_data(self):
        if os.path.exists(self._path) and os.path.getsize(self._path) > 0:
            with open(self._path, 'r') as file:
                data = json.load(file)
                return [data['username'], data['password']]
        else:
            return None


class HistoryAI:

    def __init__(self, db_path="users.db"):
        self.db_path = os.path.abspath("../../../python/users.db")
        self.history_dict = {}
        self.prompt_response_list = []
        self.current_user = None
        self.heading = None

    def dump_history(self, prompt_response_list=None):
        # If prompt_response_list is provided, use it, otherwise use the instance variable
        if prompt_response_list is not None:
            self.prompt_response_list = prompt_response_list

        if self.current_user is None:
            logger.error("User is not logged in")
            return None

        if not self.heading:
            logger.error("Heading is not set")
            return None

        if not self.prompt_response_list:
            logger.error("No prompt/response data to save")
            return None

        logger.info(f"Dumping history for user: {self.current_user}, heading: {self.heading}")
        # logger.info(f"Prompt/response list: {self.prompt_response_list}")

        try:
            with sqlite3.connect(self.db_path) as conn:
                cursor = conn.cursor()

                # retrieve user id
                cursor.execute(
                    """
                    SELECT id FROM users WHERE username = ?
                    """, (self.current_user,)
                )
                user_id_result = cursor.fetchone()

                if not user_id_result:
                    logger.error(f"User {self.current_user} not found in database")
                    return None

                user_id = user_id_result[0]
                logger.info(f"Found user ID: {user_id}")

                # Check if heading already exists for this user
                cursor.execute(
                    """
                    SELECT id FROM headings WHERE user_id = ? AND heading = ?
                    """, (user_id, self.heading)
                )
                existing_heading = cursor.fetchone()

                if existing_heading:
                    # Heading exists, update existing chat history
                    heading_id = existing_heading[0]
                    logger.info(f"Updating existing heading with ID: {heading_id}")

                    # Delete existing chat history for this heading
                    cursor.execute(
                        """
                        DELETE FROM chat_history WHERE heading_id = ?
                        """, (heading_id,)
                    )
                    logger.info(f"Deleted existing chat history for heading ID: {heading_id}")
                else:
                    # Heading doesn't exist, insert new heading
                    cursor.execute(
                        """
                        INSERT INTO headings(user_id, heading) VALUES(?, ?)
                        """, (user_id, self.heading)
                    )
                    conn.commit()

                    # retrieve heading id of newly inserted heading
                    cursor.execute(
                        """
                        SELECT id FROM headings WHERE user_id = ? AND heading = ?
                        ORDER BY id DESC LIMIT 1
                        """, (user_id, self.heading)
                    )
                    heading_id_result = cursor.fetchone()

                    if not heading_id_result:
                        logger.error("Failed to retrieve heading ID after insert")
                        return None

                    heading_id = heading_id_result[0]
                    logger.info(f"Created new heading with ID: {heading_id}")

                # Insert all prompt and response pairs
                for p_n_r in self.prompt_response_list:
                    if len(p_n_r) >= 2:
                        prompt = p_n_r[0]
                        response = p_n_r[1]
                        cursor.execute(
                            """
                            INSERT INTO chat_history(heading_id, prompt, response) VALUES(?, ?, ?)
                            """, (heading_id, prompt, response)
                        )
                    else:
                        logger.warning(f"Skipping invalid prompt/response pair: {p_n_r}")

                conn.commit()
                logger.info(f"History saved/updated successfully with heading: {self.heading}")

            return True

        except Exception as e:
            logger.error(f"Error saving history: {str(e)}")
            return None

    def load_history(self):
        if self.current_user is None:
            print("user is not logged in")
            return None
        else:
            try:
                self.history_dict = {}  # Clear any existing data

                with sqlite3.connect(self.db_path) as conn:
                    cursor = conn.cursor()
                    cursor.execute(
                        """
                        SELECT h.heading, ch.prompt, ch.response
                        FROM headings h
                        JOIN chat_history ch ON h.id = ch.heading_id
                        WHERE h.user_id = (SELECT id FROM users WHERE username = ?)
                        ORDER BY h.id, ch.id
                        """, (self.current_user,)
                    )

                    rows = cursor.fetchall()

                    if not rows:
                        logger.info(f"No history found for user: {self.current_user}")
                        return {}  # Return empty dict instead of None

                    for row in rows:
                        heading, prompt, response = row
                        if heading not in self.history_dict:
                            self.history_dict[heading] = [[], []]

                        self.history_dict[heading][0].append(prompt)
                        self.history_dict[heading][1].append(response)

                    # Debug logging
                    logger.info(f"Loaded history for user {self.current_user} with {len(self.history_dict)} headings")

                    try:
                        with open(os.path.abspath("history5577.json"), "w") as file:
                            json.dump(self.history_dict, file, indent=4)
                            logger.info(f"Wrote history to {os.path.abspath('history5577.json')}")
                    except Exception as e:
                        logger.error(f"Error writing history file: {str(e)}")

                    return self.history_dict

            except Exception as e:
                logger.error(f"Error loading history: {str(e)}")
                return {}  # Return empty dict on error


if __name__ == '__main__':
    l = LoginAI()
    l.create_table()

    hist = HistoryAI()
    hist.current_user = "pirate"
    hist.prompt_response_list = [["prompt1", "response1"], ["prompt2", "response2"], ["prompt3", "response3"],
                                 ["prompt4", "response4"], ["prompt5", "response5"]]

    hist.heading = "heading1"
    hist.dump_history()
