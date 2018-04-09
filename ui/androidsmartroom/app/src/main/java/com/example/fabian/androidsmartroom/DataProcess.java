package com.example.fabian.androidsmartroom;

/**
 * Created by Fabian on 08.04.2018.
 */

public class DataProcess {

    public static String sendDataRequest() {
        String answer = ConnectionManager.send(Constants.UI_CLIENT_DATA_REQUEST);
        String ret = answer.replace("#", "");
        return ret;
    }
}
