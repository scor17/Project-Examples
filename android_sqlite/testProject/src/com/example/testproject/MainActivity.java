package com.example.testproject;
/** 
 * Insert records into a file and check the file size
 */

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;

import android.content.Intent;
import android.os.Bundle;
import android.os.Environment;
import android.support.v7.app.ActionBarActivity;
import android.view.GestureDetector;
import android.view.GestureDetector.OnGestureListener;
import android.view.GestureDetector.SimpleOnGestureListener;
import android.view.Menu;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;


public class MainActivity extends ActionBarActivity implements View.OnClickListener {

	Intent intent;
	ListView list;
	ArrayAdapter<Float> fileSizes;

	String data = "Bob 20";
	String path = "/test.txt";
	File external = Environment.getExternalStorageDirectory();
    String sdcardPath = external.getPath();
    File file = new File(sdcardPath + path);
    
    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		intent = new Intent(this, SQLActivity.class);
		
		fileSizes = new ArrayAdapter<Float>(this, android.R.layout.simple_list_item_1); 
		list = (ListView) findViewById(R.id.listView1);
		
		list.setAdapter(fileSizes);
		Button button = (Button) findViewById(R.id.button1);
		Button button2 = (Button) findViewById(R.id.button2);
		button.setOnClickListener(this);
		button2.setOnClickListener(this);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public void onClick(View v) {
		int getid = v.getId();
		if(getid == R.id.button1) {
			try {
				if(!file.exists() || file.length() > 100){
					FileWriter fileWritter = new FileWriter(file);
					fileSizes.clear();
					fileSizes.notifyDataSetChanged();
	    			file.createNewFile();
	    		}
				
				FileWriter fileWritter = new FileWriter(file, true);
				
		        BufferedWriter bufferWritter = new BufferedWriter(fileWritter);
		        bufferWritter.write(data);
    	        bufferWritter.close();
    	        
    	        float len = file.length();
				System.out.println(len);
				this.fileSizes.add(len);
				
			} catch (Exception e) {
				android.util.Log.d("failed to save file", e.toString());
			}
		} else if(getid == R.id.button2) {
			startActivity(intent);
		}
	}
}
