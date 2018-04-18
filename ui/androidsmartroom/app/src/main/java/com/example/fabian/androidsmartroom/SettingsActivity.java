package com.example.fabian.androidsmartroom;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.Toolbar;

public class SettingsActivity extends AppCompatActivity {

    SharedPreferences pref;
    SharedPreferences.Editor editor;
    EditText firstName;
    EditText lastName;
    Switch training;
    public static EditText serverIp;
    TextView isChecked;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Toolbar toolbar = findViewById(R.id.toolbar);
        setActionBar(toolbar);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        setContentView(R.layout.activity_settings);
        training = findViewById(R.id.switch1);
        training.setChecked(true);

        if (training.isChecked()) {
            training.setText("aktiviert");
        }
        else {
            training.setText("deaktiviert");
        }

        training.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                if(b) {
                    isChecked.setText("aktiviert");
                }
                else {
                    isChecked.setText("deaktiviert");
                }
            }
        });


    }
    @Override
    public boolean onSupportNavigateUp() {
        onBackPressed();
        return true;
    }

    public void onClick(View view) {
        Intent i = new Intent(this, AddDevice.class);
        startActivity(i);
    }

    public void onClick2(View view) {
        Intent i = new Intent(this, AboutProject.class);
        startActivity(i);
    }

    public void onClick3(View view) {
        Intent i = new Intent(this, AboutProject.class);
        startActivity(i);
    }

    public void onCLick4(View view) {
        Toast.makeText(this, "TODO", Toast.LENGTH_SHORT).show();
    }

    public void onCLick5(View view) {
        Toast.makeText(this, "TODO", Toast.LENGTH_SHORT).show();
    }
}
