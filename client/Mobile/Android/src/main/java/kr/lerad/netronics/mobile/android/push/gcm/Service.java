package kr.lerad.netronics.mobile.android.push.gcm;


import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import com.google.android.gcm.GCMBaseIntentService;

import java.util.Iterator;

public class Service extends GCMBaseIntentService {
    private static final String tag = "Netronics GCM";
    private static GCM gcm;
    private GCM localGCM;

    public Service()
    {
        this("");
    }

    public Service(String project_id)
    {
        super(Service.gcm.GetProjectId());
        localGCM = Service.gcm;
    }

    public static void SetGCM(GCM gcm)
    {
        Service.gcm = gcm;
    }

    @Override
    protected void onMessage(Context context, Intent intent) {
        Bundle b = intent.getExtras();

        localGCM.PushData(b.getString("message"));

    }

    @Override
    protected void onError(Context context, String errorId) {
        Log.d(tag, "onError. errorId : "+errorId);
    }

    @Override
    protected void onRegistered(Context context, String regId) {
        Log.d(tag, "onRegistered. regId : "+regId);
        localGCM.SetRegistered(regId);
    }

    @Override
    protected void onUnregistered(Context context, String regId) {
        Log.d(tag, "onUnregistered. regId : "+regId);
        localGCM.Unregistered(regId);
    }
}
