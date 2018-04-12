package com.example.fabian.androidsmartroom;

/**
 * Created by Fabian on 08.04.2018.
 */

public class DataProcess {

    public static String sendDataRequest() {
        String answer = null;
        answer = ConnectionManager.send(Constants.UI_CLIENT_DATA_REQUEST);

        if (answer.equals(Constants.UI_CLIENT_NOT_CONNECTED)) {
            return Constants.UI_CLIENT_NOT_CONNECTED;
        }

        else {
            return answer.replace("#", "");
        }

    }
}
