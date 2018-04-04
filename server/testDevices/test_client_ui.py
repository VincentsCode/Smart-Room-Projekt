import socket
import Constants

ip = "localhost"
port = 2222

socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
socket.connect((ip, port))

msg = Constants.UI_CLIENT_DATA_REQUEST
while len(bytes(msg, "utf8")) < 32:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(2048)
s_a = str(answer, "utf8")
print(s_a)

msg = Constants.UI_CLIENT_DEVICES_LOG_REQUEST
while len(bytes(msg, "utf8")) < 32:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(2048)
s_a = str(answer, "utf8")
print(s_a)

# EXAMPLE: UI_CLIENT_COMMAND = "CMD_Computer0_2##################"
msg = Constants.UI_CLIENT_COMMAND_IDENTIFIER + "Computer0" + "_" + "1"
while len(bytes(msg, "utf8")) < 32:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(2048)
s_a = str(answer, "utf8")
print(s_a)

msg = Constants.UI_CLIENT_COMMAND_IDENTIFIER + "Light0" + "_" + "0"
while len(bytes(msg, "utf8")) < 32:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(2048)
s_a = str(answer, "utf8")
print(s_a)
