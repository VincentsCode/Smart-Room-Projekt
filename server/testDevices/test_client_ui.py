# IMPORTS
import socket
import Constants

# CONNECT TO SERVER
ip = "localhost"
port = 2222
socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
socket.connect((ip, port))

# GET INFO
# REQUEST INFO
msg = Constants.UI_CLIENT_DATA_REQUEST
while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)
s_a = str(answer, "utf8")
print(s_a)

# REQUEST LOG
msg = Constants.UI_CLIENT_DEVICES_LOG_REQUEST
while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)
s_a = str(answer, "utf8")
print(s_a)

# EDIT COMPUTER
msg = Constants.UI_CLIENT_COMMAND_IDENTIFIER + "Computer0" + "_" + "2"
while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)
s_a = str(answer, "utf8")
print(s_a)

# ADD DEVICE
msg = Constants.UI_CLIENT_ADD_DEVICE_IDENTIFIER + "Computer1_127.0.0.1_1339_3_AN_STANDBY_AUS"
while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)
s_a = str(answer, "utf8")
print(s_a)

# EDIT NEW DEVICE
msg = Constants.UI_CLIENT_COMMAND_IDENTIFIER + "Computer1" + "_" + "1"
while len(bytes(msg, "utf8")) < Constants.REQUEST_LENGTH:
    msg += "#"
socket.sendall(bytes(msg, "utf8"))
answer = socket.recv(Constants.SERVER_ANSWER_LENGTH)
s_a = str(answer, "utf8")
print(s_a)