using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    RectTransform horizon;
    [SerializeField]
    RectTransform artificialHorizonMask;
    [SerializeField]
    FlighInstrumentsManager flighInstruments;
    [SerializeField]
    TMP_Text speed;
    [SerializeField]
    TMP_Text height;
    [SerializeField]
    ScrollRect speed_scroll;
    [SerializeField]
    ScrollRect height_scroll;
    [SerializeField]
    RectTransform bank_mark;
    [SerializeField]
    RectTransform speed_mark;
    [SerializeField]
    private float maxSpeed = 400f;
    [SerializeField]
    private float minSpeed = 40f;

    private float artificialHorizonMaskHeight;
    private float groundRefPosition;
    private Vector3 speedMarkRfPosition;
    [SerializeField]
    private float pitchAnglesInArtificialHorizon;

    // Start is called before the first frame update
    [SerializeField]
    private GameObject heightScrollContentPrefab;
    [SerializeField]
    private Transform heightScrollContent;
    [SerializeField]
    private float minHeight = 0f;
    [SerializeField]
    private float maxHeight= 40000f;
    [SerializeField]
    private float heightPerThousandFeets = 160f;
    [SerializeField]
    private float heightScrooShift = -22f;

    //<STAB TRIM UI>
    [SerializeField]
    private RectTransform trimScale;
    [SerializeField]
    private RectTransform trimMark;
    [SerializeField]
    private float trimScaleFractionAtImageInHeight;
    private float trimMarkMinPos;
    private float trimMarkMaxPos;
    //</STAB TRIM UI>

    //<FLAPS UI>
    private int currentFlapsPos;
    private float flapsMarkMinPos;
    private float flapsMarkMaxPos;
    [SerializeField]
    private RectTransform flapScale;
    [SerializeField]
    private RectTransform flapMark;
    [SerializeField]
    private float flapScaleFractionAtImageInHeight;
    [SerializeField]
    private float flapLeverAnimationTime = 1.0f;
    private float flapLeverAnimationElapsed = 0.0f;
    [SerializeField]
    private int flapLevelsNumber = 9;
    //</FLAPS UI>

    //<GEAR UI>
    private bool gearExtended = true;
    private float gearMarkExtendedPos;
    private float gearMarkRetractedPos;
    [SerializeField]
    private RectTransform gearScale;
    [SerializeField]
    private RectTransform gearMark;
    [SerializeField]
    private float gearScaleFractionAtImageInHeight;
    [SerializeField]
    private float gearLeverAnimationTime = 1.0f;
    private float gearLeverAnimationElapsed = 0.0f;
    //</GEAR UI>

    //<ENGINE UI>
    [SerializeField]
    private RectTransform[] EngineArrow;
    [SerializeField]
    private RectTransform[] EngineSecondaryArrow;
    [SerializeField]
    private float EngineArrowAngleSpan;
    private Quaternion[] minEngineArrowAngle;
    private Quaternion[] maxEngineArrowAngle;
    [SerializeField]
    TMP_Text[] EngineText;
    private int enginesCount;
    [SerializeField]
    private GameObject flightInstrumentsPanel;
    private bool flightInstrumentPanelIsActive;
    //</ENGINE UI>

    //<ILS directors>
    [SerializeField]
    private RectTransform horizontal_ILS_Bar;
    [SerializeField]
    private RectTransform vertical_ILS_Bar;
    //</ILS directors>

    //<MENU>
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private TMP_Text dayTimeText;
    [SerializeField]
    private Slider dayTimeSlider;
    [SerializeField]
    private TMP_Text windSpeedText;
    [SerializeField]
    private Slider windSpeedSlider;
    [SerializeField]
    private TMP_Text windAzimuthText;
    [SerializeField]
    private Slider windAzimuthSlider;
    //</MENU>
    void Start()
    {
        menu.SetActive(false);
        dayTimeSlider.value = 12f;
        windAzimuthSlider.value = 0f;
        windSpeedSlider.value = 0f;
        ChangeDayTimeValue();
        ChangeWindAzimuthValue();
        ChangeWindSpeedValue();

        groundRefPosition = horizon.anchoredPosition.y;
        speedMarkRfPosition = speed_mark.anchoredPosition;
        trimMarkMinPos = trimMark.anchoredPosition.y;
        trimMarkMaxPos = trimMarkMinPos - trimScale.rect.height * trimScaleFractionAtImageInHeight;
        flapsMarkMinPos = flapMark.anchoredPosition.y;
        flapsMarkMaxPos = flapsMarkMinPos - flapScale.rect.height * flapScaleFractionAtImageInHeight;
        gearMarkExtendedPos = gearMark.anchoredPosition.y;
        gearMarkRetractedPos = gearMarkExtendedPos + gearScale.rect.height * gearScaleFractionAtImageInHeight;

        enginesCount = EngineArrow.Length;
        minEngineArrowAngle = new Quaternion[enginesCount];
        maxEngineArrowAngle = new Quaternion[enginesCount];
        for(int i=0;i<enginesCount;i++)
            minEngineArrowAngle[i] = EngineArrow[i].localRotation;
       // Quaternion engineArrowSpan = Quaternion.Euler(0, 0, EngineArrowAngleSpan);
      //  maxEngineArrowAngle = minEngineArrowAngle * engineArrowSpan;
       // maxEngineArrowAngle = new Quaternion(-maxEngineArrowAngle.x, -maxEngineArrowAngle.y, -maxEngineArrowAngle.z, -maxEngineArrowAngle.w);

        for (int i = 0; i < 40; i++)
        {
            GameObject newItem = Instantiate(heightScrollContentPrefab, heightScrollContent);
            RectTransform newItemRectTransform = newItem.GetComponent<RectTransform>();
            Vector2 pos = newItemRectTransform.anchoredPosition;
            pos.x = 0f;
            pos.y = (i+1) * heightPerThousandFeets + heightScrooShift;
            newItemRectTransform.anchoredPosition = pos;
            // Customize the new item based on your prefab and data
            // For example, if your prefab has a Text component:
            
            for(int j = 0; j < newItem.transform.childCount; j++)
            {
                Transform child = newItem.transform.GetChild(j);
                TMP_Text text = child.gameObject.GetComponent<TMP_Text>();
                text.text = (i).ToString("F0");
                if (i == -1)
                    text.text = "";
            }
        }

        flightInstrumentsPanel.SetActive(false);
        flightInstrumentPanelIsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = horizon.eulerAngles;
        rot.z = flighInstruments.BankAngle;
        horizon.eulerAngles = rot;

        Vector3 bank_mark_rot = bank_mark.eulerAngles;
        bank_mark_rot.z = flighInstruments.BankAngle;
        bank_mark.eulerAngles = bank_mark_rot;

        artificialHorizonMaskHeight = artificialHorizonMask.rect.height;
        float heightPerDegree = artificialHorizonMaskHeight / pitchAnglesInArtificialHorizon;
        Debug.Log("UI Height per degree: " + heightPerDegree);
        float horizonShift_y = heightPerDegree * flighInstruments.PitchAngle*Mathf.Cos(flighInstruments.BankAngle*Mathf.Deg2Rad);
        float horizonShift_x = -heightPerDegree * flighInstruments.PitchAngle * Mathf.Sin(flighInstruments.BankAngle * Mathf.Deg2Rad);
        Debug.Log("UI pitch angle: " + flighInstruments.PitchAngle);
        Vector3 pos = horizon.anchoredPosition;
        pos.y = groundRefPosition + horizonShift_y;
        pos.x = horizonShift_x;
        horizon.anchoredPosition = pos;

        Debug.Log("UI speed: " + flighInstruments.Velocity.magnitude);
        string speed_text = (1.94f*(flighInstruments.Velocity.magnitude)).ToString("F0");
        speed.text = speed_text;

        float height_f = (3.28f * flighInstruments.Altitude);
        float height_normalized = Mathf.InverseLerp(minHeight, maxHeight, height_f);
        string height_text = (height_f).ToString("F0");
        height.text = height_text;
        height_scroll.verticalNormalizedPosition = height_normalized;

        float speed_f = 1.94f * (flighInstruments.Velocity.magnitude);
        float normalized = Mathf.InverseLerp(minSpeed, maxSpeed, speed_f);
        speed_scroll.verticalNormalizedPosition =  normalized;

        Vector3 relativeSpeed = flighInstruments.Velocity;
        float hor_angle = Vector3.SignedAngle(Vector3.forward, relativeSpeed, Vector3.up);
        float ver_angle = Vector3.SignedAngle(Vector3.forward, relativeSpeed, Vector3.right);
        if (hor_angle > 90f)
            hor_angle -= 180f;
        if (hor_angle < -90f)
            hor_angle += 180f;
        if (ver_angle > 90f)
            ver_angle -= 180f;
        if (ver_angle < -90f)
            ver_angle += 180f;
        float hor_speed_marker_shift = hor_angle * heightPerDegree;
        float ver_speed_marker_shift = -ver_angle * heightPerDegree;
        Vector3 speed_mark_pos = speedMarkRfPosition;
        speed_mark_pos.y += ver_speed_marker_shift;
        speed_mark_pos.x += hor_speed_marker_shift;
        speed_mark.anchoredPosition = speed_mark_pos;
        if (relativeSpeed.magnitude > 20f)
            speed_mark.gameObject.GetComponent<Image>().enabled = true;
        else
            speed_mark.gameObject.GetComponent<Image>().enabled = false;

        //<STAB TRIM UI>
        float degree = Mathf.InverseLerp(0, 15, flighInstruments.StabilizerTrimAngle);
        float trim_mark_pos_y = Mathf.Lerp(trimMarkMinPos, trimMarkMaxPos, degree);
        Vector2 trim_mark_pos = trimMark.anchoredPosition;
        trim_mark_pos.y = trim_mark_pos_y;
        trimMark.anchoredPosition = trim_mark_pos;
        //</STAB TRIM UI>

        //<ENGINE THRUST UI>
        for(int i=0;i<enginesCount;i++)
        {
            float angle = flighInstruments.GetEngineLevel(i) * EngineArrowAngleSpan;
            Quaternion engine_arrow_rotation = Quaternion.Euler(0, 0, angle);
            EngineArrow[i].localRotation = minEngineArrowAngle[i] * engine_arrow_rotation;
            float angle2 = 0f;
            if (angle < -110f)
                angle2 = angle + 110f;
            Quaternion engine_secondary_arrow_rotation = Quaternion.Euler(0, 0, angle2);
            EngineSecondaryArrow[i].localRotation = minEngineArrowAngle[i] * engine_secondary_arrow_rotation;

            string engine_text = (100f * flighInstruments.GetEngineLevel(i)).ToString("F1");
            EngineText[i].text = engine_text;
        }

        //</ENGINE THRUST UI>

        //<ILS directors>
        if (!flighInstruments.ILS_captured)
        {
            horizontal_ILS_Bar.gameObject.SetActive(false);
            vertical_ILS_Bar.gameObject.SetActive(false);
        }
        else
        {
            horizontal_ILS_Bar.gameObject.SetActive(true);
            vertical_ILS_Bar.gameObject.SetActive(true);
            float hor_ILS_director_shift = flighInstruments.Angle_ILS_hor * heightPerDegree;
            float ver_ILS_director_shift = flighInstruments.Angle_ILS_ver * heightPerDegree;

            Vector2 hor_ILS_bar_pos = horizontal_ILS_Bar.anchoredPosition;
            hor_ILS_bar_pos.x = hor_ILS_director_shift;
            horizontal_ILS_Bar.anchoredPosition = hor_ILS_bar_pos;

            Vector2 ver_ILS_bar_pos = vertical_ILS_Bar.anchoredPosition;
            ver_ILS_bar_pos.y = ver_ILS_director_shift;
            vertical_ILS_Bar.anchoredPosition = ver_ILS_bar_pos;
        }
        //</ILS directors>

        //<MENU>
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenCloseMenu();
        //<MENU>
    }

    public void Flaps(int pos)
    {
        if (currentFlapsPos == pos)
            return;
        StopAllCoroutines();
        StartCoroutine(MoveFlapsLever(pos));
    }
    IEnumerator MoveFlapsLever(int pos)
    {
        flapLeverAnimationElapsed = 0.0f;
        float start_pos = Mathf.Lerp(flapsMarkMinPos, flapsMarkMaxPos, (float)currentFlapsPos / (float)(flapLevelsNumber-1));
        float end_pos = Mathf.Lerp(flapsMarkMinPos, flapsMarkMaxPos, (float)pos/ (float)(flapLevelsNumber - 1));

        while (flapLeverAnimationElapsed < flapLeverAnimationTime)
        {
            flapLeverAnimationElapsed += Time.deltaTime;
            float degree = flapLeverAnimationElapsed / flapLeverAnimationTime;

            float pos_y = Mathf.Lerp(start_pos, end_pos, degree);
            Vector2 flap_mark_pos = flapMark.anchoredPosition;
            flap_mark_pos.y = pos_y;
            flapMark.anchoredPosition = flap_mark_pos;
          
            yield return null;
        }

        currentFlapsPos = pos;
    }
    public void ToggleGear(bool extended)
    {
        StopAllCoroutines();
        StartCoroutine(MoveGearLever(extended));
    }
    IEnumerator MoveGearLever(bool toExtended)
    {
        gearLeverAnimationElapsed = 0.0f;
        //float start_pos = Mathf.Lerp(flapsMarkMinPos, flapsMarkMaxPos, (float)currentFlapsPos / (float)(flapLevelsNumber - 1));
        //float end_pos = Mathf.Lerp(flapsMarkMinPos, flapsMarkMaxPos, (float)pos / (float)(flapLevelsNumber - 1));
        float start_pos = gearMark.anchoredPosition.y;
        float end_pos = 0;
        if (toExtended)
            end_pos = gearMarkExtendedPos;
        else
            end_pos = gearMarkRetractedPos;

        while (gearLeverAnimationElapsed < gearLeverAnimationTime)
        {
            gearLeverAnimationElapsed += Time.deltaTime;
            float degree = gearLeverAnimationElapsed /gearLeverAnimationTime;

            float pos_y = Mathf.Lerp(start_pos, end_pos, degree);
            Vector2 gear_mark_pos = gearMark.anchoredPosition;
            gear_mark_pos.y = pos_y;
            gearMark.anchoredPosition = gear_mark_pos;

            yield return null;
        }
    }
    public void ToggleUIPanel()
    {
        flightInstrumentPanelIsActive = !flightInstrumentPanelIsActive;
        flightInstrumentsPanel.SetActive(flightInstrumentPanelIsActive);
    }
    public void OpenCloseMenu()
    {
        if (menu.gameObject.active)
            menu.SetActive(false);
        else
            menu.SetActive(true);
    }
    public void ChangeDayTimeValue()
    {
        float newDayTime = dayTimeSlider.value;
        MainManager.Instance.DayTime = newDayTime;
        int hours = (int)Mathf.Floor(newDayTime);
        float minutes = (int)Mathf.Floor((newDayTime - hours) * 60);
        string time = hours.ToString() + ":" + minutes.ToString();
        dayTimeText.text = time;

    }
    public void ChangeWindSpeedValue()
    {
        MainManager.Instance.WindAmplitude = windSpeedSlider.value;
        windSpeedText.text = ((int)Mathf.Floor(windSpeedSlider.value)).ToString();
    }
    public void ChangeWindAzimuthValue()
    {
        MainManager.Instance.WindAzimuth = windAzimuthSlider.value;
        windAzimuthText.text = ((int)Mathf.Floor(windAzimuthSlider.value)).ToString();
    }
}
