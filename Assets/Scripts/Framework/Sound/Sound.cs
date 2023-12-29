using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound : Singleton<Sound>
{
    public string ResourceDir = "Sounds";

    // 背景音乐组件
    AudioSource bgSound;
    // 音效组件
    AudioSource effectSound;

    protected override void Awake()
    {
        base.Awake();
        // 添加背景音乐组件
        bgSound = gameObject.AddComponent<AudioSource>();
        bgSound.playOnAwake = false;
        bgSound.loop = true;

        effectSound = gameObject.AddComponent<AudioSource>();
    }

    // 音乐大小
    public float BgVolume
    {
        get { return bgSound.volume; }
        set { bgSound.volume = value; }
    }

    // 音效大小
    public float EffectVolume
    {
        get { return effectSound.volume; }
        set { effectSound.volume = value; }
    }

    // 播放音乐
    public void PlayBgSound(string name)
    {
        // 前后音乐一样处理
        string oldName;
        if (bgSound.clip != null)
            oldName = bgSound.clip.name;
        else
            oldName = "";

        // 播放音乐一样直接返回
        if (name == oldName)
        {
            return;
        }

        // 判断音乐文件是否存在
        if (!System.IO.File.Exists(ResourceDir + "/" + name))
        {
            Debug.Log("没有找到音乐文件：" + name);
            return;
        }
        bgSound.clip = Resources.Load<AudioClip>(ResourceDir + "/" + name);
        bgSound.Play();
    }

    // 播放音效
    public void PlayEffectSound(string name)
    {
        if (!System.IO.File.Exists(ResourceDir + "/" + name))
        {
            Debug.Log("没有找到音效文件：" + name);
            return;
        }
        // 播放
        AudioClip clip = Resources.Load<AudioClip>(ResourceDir + "/" + name);
        effectSound.PlayOneShot(clip);
    }

    // 停止播放音乐
    public void StopBgSound()
    {
        bgSound.Stop();
    }
}