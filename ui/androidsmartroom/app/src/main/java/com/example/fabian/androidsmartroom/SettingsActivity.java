package com.example.fabian.androidsmartroom;

import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.Editable;
import android.widget.EditText;
import android.widget.Switch;

import static android.provider.Telephony.Mms.Part.FILENAME;

public class SettingsActivity extends AppCompatActivity {

    private static final String VAL_KEY = "&quot;ValueKey&quot;";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_settings);
        SharedPreferences sharedPrefs = getSharedPreferences(FILENAME, 0);
        setTitle("Einstellungen");
        EditText firstName = findViewById(R.id.editTextFirstName);
        EditText lastName = findViewById(R.id.editTextLastName);
        Switch training = findViewById(R.id.switch1);

        Editable nameFirst = firstName.getText();
        Editable nameLast = lastName.getText();
        boolean isSwitchChecked = training.isChecked();
    }


}
