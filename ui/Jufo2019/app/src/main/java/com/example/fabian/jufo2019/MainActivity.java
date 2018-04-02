package com.example.fabian.jufo2019;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;


public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
    }

     public void openHeaterActivity(View view) {
         Intent i = new Intent(this, heaterActivity.class);
         startActivity(i);
    }

    public void openlightActivity(View view) {
        Intent i = new Intent(this, lightActivity.class);
        startActivity(i);
    }

}
