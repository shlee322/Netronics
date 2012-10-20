package kr.lerad.androidtest;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.util.Log;
import kr.lerad.netronics.mobile.android.Mobile;
import kr.lerad.netronics.mobile.android.RecvOnListener;
import kr.lerad.netronics.mobile.android.push.gcm.GCM;

public class MyActivity extends Activity {
    Mobile mobile;
    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);

        //setContentView(R.layout.main);
        try {
            //KeyStore ks = KeyStore.getInstance(KeyStore.getDefaultType());
            //ks.load(getResources().openRawResource(R.raw.public_key), "asdf".toCharArray());

            mobile = new Mobile(1, "192.168.105.129", 7777);
            mobile.SetAuthFile(this.getFileStreamPath("netronics_auth").getPath());
            mobile.On("hi", new RecvOnListener() {
                @Override
                public void On(Mobile mobile, Object o) {
                    mobile.Emit("hi2", o);
                }
            });
            mobile.On("push_test", new RecvOnListener() {
                @Override
                public void On(Mobile mobile, Object o) {
                    Log.d("Netronics Push", o.toString());
                }
            });
            mobile.AddPush(new GCM(this, "568476072992"));
            mobile.Run();
            mobile.Emit("hi", "test");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
