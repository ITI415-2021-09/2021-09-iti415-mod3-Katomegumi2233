using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoleBulletController : MonoBehaviour
{
    private int bullets = 100;

    
    static public int score = 0;

    
    public Rigidbody bullet;

    
    private GameObject firePoint;

    
    public Texture2D texture;

    
    private int blood = 100;

    
    private bool isShowBlood = true;

    
    public Texture2D bloodBgTexture;

    
    public Texture2D bloodTexture;

    
    public Texture2D AllRed;

    
    private float howAlpha;

    
    public GameObject Gun;

    float attackTimeCounter = 0;

    public Image hpValue;
    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        firePoint = GameObject.Find("firePoint");
    }

    // Update is called once per frame
    void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;

        
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) && bullets > 0)
        {
            attackTimeCounter += Time.deltaTime;

            if (attackTimeCounter < 0.15f)
                return;

            attackTimeCounter = 0;
            
            
            Vector3 target = ray.GetPoint(20);
            
            
            
            Rigidbody clone = (Rigidbody)Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
            
            clone.velocity = (target - firePoint.transform.position) * 3;
            
            Gun.SendMessage("shootAudio");

            
            if (Physics.Raycast(ray, out hit, 100, 1 << 9))
            {
                
                Debug.Log(hit.normal);
                
                Destroy(hit.collider);
                
                score++;
                //Destroy(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<EnemyController>().dead();
            }
        }
        
        firePoint.transform.LookAt(Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(20));
        
        if (score >= 56)
        {
            Debug.Log("You Win!");
            Application.LoadLevel(8);
        }

        
        if (blood <= 0 || bullets <= 0)
        {
            Debug.Log("Game Over!");
            Application.LoadLevel(9);

        }
    }

    
    public void addBlood()
    {
        if (blood > 50)
        {
            blood = 100;
        }
        else
        {
            blood += 50;
        }
    }

    
    public void reduceBlood(int attackType)
    {
        switch (attackType)
        {
            case 6:
                blood -= 6;
                break;
            case 8:
                blood -= 8;
                break;
            case 10:
                blood -= 10;
                break;
            default:
                Debug.Log("blood error");
                break;
        }
    }

    void OnGUI()
    {
        
        Rect rect = new Rect(Input.mousePosition.x - (texture.width / 2),
        Screen.height - Input.mousePosition.y - (texture.height / 2),
        texture.width, texture.height);
        GUI.DrawTexture(rect, texture);
        
        Color alpha = GUI.color;
        howAlpha = (100.0f - blood) / 120.0f;
        if (howAlpha < 0.42)
        {
            howAlpha = 0;
        }
        alpha.a = howAlpha;
        GUI.color = alpha;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), AllRed);

        hpValue.fillAmount = blood * 0.01f;
        scoreText.text = score.ToString();
    }
}
