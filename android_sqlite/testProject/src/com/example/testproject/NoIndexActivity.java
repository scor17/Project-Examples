package com.example.testproject;

import java.io.File;
import java.sql.Timestamp;

import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteException;
import android.os.Bundle;
import android.os.Environment;
import android.support.v7.app.ActionBarActivity;
import android.view.Menu;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;

/** 
 * Test latency in retrieving an arbitrary record from a database
 * 
 * REQUIRED: MUST PUSH BIGDB ONTO YOUR DEVICE (location testProject/BIGDB)
 */

public class NoIndexActivity extends ActionBarActivity implements OnClickListener {

	Intent intent;
	Intent intent2;
	ListView list;
	ArrayAdapter<String> fileSizes;

	String path = "/BIGDB1";
	File external = Environment.getExternalStorageDirectory();
    String sdcardPath = external.getPath();
    File file = new File(sdcardPath + path);
    SQLiteDatabase db;
    String queryString;
    
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_no_index);
		intent = new Intent(this, SQLActivity.class);
		intent2 = new Intent(this, IndexActivity.class);
		
		db = openOrCreateDatabase(sdcardPath + path, Context.MODE_PRIVATE, null);
		
		fileSizes = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1); 
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
		int getid = v.getId();
		Cursor cursor;
		if(getid == R.id.button1) {
			try {
				db = openOrCreateDatabase(sdcardPath + path, Context.MODE_PRIVATE, null);
				db.beginTransaction();
				int time = (int) (System.currentTimeMillis());
				Timestamp tsTemp = new Timestamp(time);
				String ts =  tsTemp.toString();
				cursor = db.rawQuery("Select * FROM students WHERE grade == '50';", null);
				int time2 = (int) (System.currentTimeMillis());
				tsTemp = new Timestamp(time2);
				String ts2 =  tsTemp.toString();
				
				db.setTransactionSuccessful();
				db.endTransaction();
				this.fileSizes.add(ts);
				this.fileSizes.add(ts2);
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
