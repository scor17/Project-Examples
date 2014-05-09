package com.example.testproject;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteException;
import android.database.sqlite.SQLiteOpenHelper;
import android.os.Bundle;
import android.os.Environment;
import android.support.v7.app.ActionBarActivity;
import android.view.Menu;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
/** 
 * Insert records into an sqlite database file and check the file size
 */
public class SQLActivity extends ActionBarActivity implements View.OnClickListener {
	
	Intent intent;
	Intent intent2;
	ListView list;
	ArrayAdapter<Float> fileSizes;

	String path = "/studentDB";
	File external = Environment.getExternalStorageDirectory();
    String sdcardPath = external.getPath();
    File file = new File(sdcardPath + path);
    SQLiteDatabase db;
    String queryString;
    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_sql);
		intent = new Intent(this, MainActivity.class);
		intent2 = new Intent(this, NoIndexActivity.class);
		
		db = openOrCreateDatabase(sdcardPath + path, Context.MODE_PRIVATE, null);
		
		queryString = "DROP TABLE IF EXISTS students";
		db.execSQL(queryString);
		
		queryString = "CREATE TABLE students (name varchar(30), grade varchar(3))";
		db.execSQL(queryString);
		
		fileSizes = new ArrayAdapter<Float>(this, android.R.layout.simple_list_item_1); 
		list = (ListView) findViewById(R.id.listView1);
		list.setAdapter(fileSizes);
		
		Button button = (Button) findViewById(R.id.button1);
		Button button2 = (Button) findViewById(R.id.button2);
		Button button3 = (Button) findViewById(R.id.button3);
		button.setOnClickListener(this);
		button2.setOnClickListener(this);
		button3.setOnClickListener(this);
		db.close();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public void onClick(View v) {
		int getid = v.getId();//db.open();
		if(getid == R.id.button1) {
			try {
				db = openOrCreateDatabase(sdcardPath + path, Context.MODE_PRIVATE, null);
				db.beginTransaction();
				queryString = "INSERT INTO students(name, grade) values ('Bob', '50')";
				db.execSQL(queryString);
				db.setTransactionSuccessful();
				db.endTransaction();
    	        float len = file.length();
				System.out.println(len);
				this.fileSizes.add(len);
				db.close();
			} catch (SQLiteException e) {
				android.util.Log.d("nope", e.toString());
			} catch (Exception e) {
				android.util.Log.d("nope", e.toString());
			}
		} else if(getid == R.id.button2) {
			startActivity(intent);
		} else if(getid == R.id.button3) {
			startActivity(intent2);
		}
	}
}
