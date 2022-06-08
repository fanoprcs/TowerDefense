using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageBox : Singleton<MessageBox>
{
    
    [SerializeField]
    private Message message;
    [SerializeField]
    private Button[] option;
    public Button[] Option
    {
        get { return option; }
    }
    public Text Message;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
