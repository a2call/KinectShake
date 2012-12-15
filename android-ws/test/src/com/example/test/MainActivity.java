package com.example.test;
import java.util.Timer;
import java.util.TimerTask;
import android.media.Ringtone;
import android.media.RingtoneManager;
import android.net.Uri;
import android.os.Bundle;
import android.os.Vibrator;
import android.app.Activity;
import android.content.Context;
import android.view.Menu;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageButton;

public class MainActivity extends Activity {
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);	
		
		MyTimerTask myTask = new MyTimerTask(this);
		
        Timer myTimer = new Timer();
        myTimer.schedule(myTask,1000,500);
	}
	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.activity_main, menu);
		return true;
	}
	public void vibrate(int duration)
	{
		Vibrator vib = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
		vib.vibrate(duration);
	}
	public void btn6Click(View v){
		Thread t = new Thread(new SocketClient("hydrogen",this));
		t.start();
		//Vibrator vib = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
		//vib.vibrate(300);
		Uri notification = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);
		Ringtone r = RingtoneManager.getRingtone(getApplicationContext(), notification);
		r.play();
	}
	
	public void btn7Click(View v){
		Thread t = new Thread(new SocketClient("oxygen",this));
		t.start();
		//Vibrator vib = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
		//vib.vibrate(300);
		//Uri notification = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);
		//Ringtone r = RingtoneManager.getRingtone(getApplicationContext(), notification);
		//r.play();
		
	}
	
	public void btn8Click(View v){
		Thread t = new Thread(new SocketClient("drop",this));
		t.start();
		//Vibrator vib = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);
		//vib.vibrate(300);
		//Uri notification = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);
		//Ringtone r = RingtoneManager.getRingtone(getApplicationContext(), notification);
		//r.play();
	}
	
}


class MyTimerTask extends TimerTask{
	MainActivity mac;
	public MyTimerTask(MainActivity m){
		mac = m;
	}
	public void  run(){
		Thread t = new Thread(new SocketClient("check",mac));
		t.start();				
	}
}
