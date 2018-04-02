package com.example.fabian.jufo2019;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;

public class lightActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_light);
    }
    public void openMain(View view) {
        Intent i = new Intent(this, MainActivity.class);
        startActivity(i);
    }
}
