package kr.lerad.netronics.mobile.android.push.gcm;

import android.content.Context;
import com.fasterxml.jackson.core.JsonFactory;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.google.android.gcm.GCMRegistrar;
import kr.lerad.netronics.mobile.android.Mobile;
import kr.lerad.netronics.mobile.android.push.Push;

import java.io.IOException;
import java.util.Map;

/**
 * Created with IntelliJ IDEA.
 * User: Sanghyuck
 * Date: 12. 10. 19
 * Time: 오후 10:07
 * To change this template use File | Settings | File Templates.
 */
public class GCM implements Push {
    private static final ObjectMapper objectMapper = new ObjectMapper(new JsonFactory());
    private Context context;
    private String project_id;
    private Mobile _mobile;

    public GCM(Context context, String project_id)
    {
        this.context = context;
        this.project_id = project_id;
    }

    public void SetMobile(Mobile mobile)
    {
        _mobile = mobile;
    }

    public String GetProjectId()
    {
        return project_id;
    }

    public void Run()
    {
        Service.SetGCM(this);
        GCMRegistrar.checkDevice(this.context);
        GCMRegistrar.checkManifest(this.context);
        registerToken();
    }



    private void registerToken() {
        final String regId = GCMRegistrar.getRegistrationId(this.context);

        if ("".equals(regId)) {
            GCMRegistrar.register(this.context, this.project_id);
        }else
        {
            _mobile.AddPushAuth("GCM", regId);
        }
    }

    private void unregisterToken() {
        if (GCMRegistrar.isRegistered(this.context)) {
            GCMRegistrar.unregister(this.context);
        }
    }

    public void SetRegistered(String id)
    {
        _mobile.AddPushAuth("GCM", id);
    }

    public void Unregistered(String regId) {
        _mobile.RemovePushAuth("GCM");
    }

    public void PushData(String message) {
        try {
            Map<?, ?> map = objectMapper.readValue(message, Map.class);
            _mobile.Call((String)map.get("type"), map.get("arg"));
        } catch (IOException e) {
            e.printStackTrace();
        }

    }
}
