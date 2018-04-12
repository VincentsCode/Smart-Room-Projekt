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
    EditText serverIp;

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
        firstName = findViewById(R.id.editTextFirstName);
        lastName = findViewById(R.id.editTextLastName);
        training = findViewById(R.id.switch1);
        serverIp = findViewById(R.id.editTextServerIp);

        loadValues();

    }

    public void saveValues() {

        String nameFirst = firstName.getText().toString();
        String nameLast = lastName.getText().toString();
        String ipServer = serverIp.getText().toString();
        boolean isSwitchChecked = training.isChecked();

        editor.putString("FirstName", nameFirst);
        editor.putString("LastName", nameLast);
        editor.putString("ServerIP", ipServer);
        editor.putBoolean("Training", isSwitchChecked);

        editor.apply();
        editor.commit();
    }

    public void loadValues() {

        firstName.setText(pref.getString("FirstName", ""));
        lastName.setText(pref.getString("LastName", ""));
        serverIp.setText(pref.getString("ServerIP", ""));

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
