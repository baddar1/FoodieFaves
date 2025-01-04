import http.server
import socketserver
import os

# Automatically serve the directory where the script is located
script_directory = os.path.dirname(os.path.abspath(__file__))
os.chdir(script_directory)

# Port for the server
PORT = 8000  # You can change this to any port if needed

# Define the request handler
Handler = http.server.SimpleHTTPRequestHandler

try:
    with socketserver.TCPServer(("", PORT), Handler) as httpd:
        print(f"Serving files from: {script_directory}")
        print(f"Server running at: http://localhost:{PORT}")
        print("Press Ctrl+C to stop the server.")
        httpd.serve_forever()
except KeyboardInterrupt:
    print("\nServer stopped.")
