package com.example.fabian.androidsmartroom;

/**
 * Created by Fabian on 12.04.2018.
 */

public class DevicesManager {

    int statesAsInt;
    int maxStates;
    String name;
    String statesAsString;

    public DevicesManager(int a, int b, String c) {
        this.statesAsInt = a;
        this.maxStates = b;
        this.name = c;
    }

    public String switchState(){

        if (statesAsInt < maxStates) {
            statesAsInt = statesAsInt + 1;
        }
        else if (statesAsInt == maxStates) {
            statesAsInt = 0;
        }
        statesAsString = String.valueOf(statesAsInt);
        String msg = Constants.UI_CLIENT_COMMAND_IDENTIFIER + name + "_" + statesAsString;
        ConnectionManager.send(msg);
        return statesAsString;
    }

}
