package com.example.fabian.androidsmartroom;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.widget.EditText;
import android.widget.Switch;

import static android.provider.Telephony.Mms.Part.FILENAME;

public class SettingsActivity extends AppCompatActivity {

    SharedPreferences pref;
    SharedPreferences.Editor editor;
    EditText firstName;
    EditText lastName;
    Switch training;
    public static EditText serverIp;

    Context c;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_settings);
        SharedPreferences sharedPrefs = getSharedPreferences(FILENAME, 0);
        setTitle("Einstellungen");
        c = this;
        pref = getSharedPreferences("Einstellungen", 0);
        editor = pref.edit();
        training = findViewById(R.id.switch1);
        loadValues();

    }

    public void saveValues() {

        boolean isSwitchChecked = training.isChecked();

        editor.putBoolean("Training", isSwitchChecked);

        editor.apply();
        editor.commit();
    }

    public void loadValues() {

        training.setChecked(pref.getBoolean("Training", true));

    }

    public String getIP() {
        return serverIp.getText().toString();
    }

    @Override
    protected void onPause() {
        super.onPause();
        saveValues();
    }

    @Override
    protected void onStop() {
        super.onStop();
        saveValues();
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        saveValues();
    }


}
