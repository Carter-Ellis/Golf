using UnityEditor;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundEffect
{

    private EventInstance eventInst;
    private EventReference eventRef;

    public bool isPlaying {
        get
        {
            return eventInst.isValid();
        }
    }

    public SoundEffect(EventReference sfx)
    {

        eventRef = sfx;

    }

    public void play(MonoBehaviour component)
    {
        play(component.gameObject);
    }

    public void play(GameObject source)
    {
            
        eventInst = RuntimeManager.CreateInstance(eventRef);
        setPosition(source);
        eventInst.start();

    }

    public void stop()
    {

        eventInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInst.release();

    }

    public void updatePosition(MonoBehaviour component)
    {
        updatePosition(component.gameObject);
    }

    public void updatePosition(GameObject source)
    {

        setPosition(source);

    }

    private void setPosition(GameObject source)
    {
        Vector3 pos = Vector3.zero;
        if (source != null)
        {
            pos = source.transform.position;
        }
        eventInst.set3DAttributes(RuntimeUtils.To3DAttributes(pos));
    }

}