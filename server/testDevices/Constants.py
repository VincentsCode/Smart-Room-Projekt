# CODE-CONSTANTS
REQUEST_LENGTH = 64
SERVER_ANSWER_LENGTH = 2048

# ANSWER-CODES
ANSWER_NEGATIVE = "0"
ANSWER_POSITIVE = "1"
ANSWER_BYTES_LENGTH = len(bytes(ANSWER_POSITIVE, "utf8"))

# REQUEST-CODES
UI_CLIENT_DATA_REQUEST =         "0000000000000000000000000000000000000000000000000000000000000097"  # DONE
UI_CLIENT_DEVICES_LOG_REQUEST =  "0000000000000000000000000000000000000000000000000000000000000087"  # DONE
UI_CLIENT_MOVEMENT_LOG_REQUEST = "0000000000000000000000000000000000000000000000000000000000000077"  # RNN_TODO
UI_CLIENT_SENSOR_DATA_REQUEST =  "0000000000000000000000000000000000000000000000000000000000000067"  # SENSOR_TODO
UI_CLIENT_RNN_HABITS_REQUEST =   "0000000000000000000000000000000000000000000000000000000000000057"  # RNN_TODO

# COMMAND-IDENTIFIER
UI_CLIENT_COMMAND_IDENTIFIER = "CMD_"                                                                # DONE
UI_CLIENT_SYSTEM_COMMAND_IDENTIFIER = "SYS_"                                                         # RNN_TODO
UI_CLIENT_ADD_DEVICE_IDENTIFIER = "ADD_"                                                             # DONE



# EXAMPLES
# UI_CLIENT_COMMAND:        "CMD_Computer0_1"
# UI_CLIENT_SYSTEM_COMMAND: "SYS_NOT_IMPLEMENTED_YET"
# UI_CLIENT_ADD_DEVICE:     "ADD_Computer1_192.168.2.110_1338_3_AN_STANDBY_AUS"
# LENGTH:                   "0000000000000000000000000000000000000000000000000000000000000000"
