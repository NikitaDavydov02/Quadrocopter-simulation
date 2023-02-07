using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneFlightController : MonoBehaviour
{
    public float generalLevel = 0;
    public float generalLevelChangingSpeed = 1f;
    List<float> engineLevels = new List<float>();

    public List<List<float>> InertiaTensor;

    private List<List<float>> invertedInertiaTensor;

    [SerializeField]
    List<EngineForce> engines;
    [SerializeField]
    public GravityForce gravityForce;
    [SerializeField]
    public List<WingForce> wings;
    [SerializeField]
    List<ResistanceForce> resistanceForces;

    private Vector3 ForceToCenterOfMass;
    private Vector3 MomentInCoordinatesTranslatedToCenterOfMass;

    private Vector3 velocityOfCenterMass = Vector3.zero;
    private Vector3 AbsoluteAngularVelocity = Vector3.zero;
    private Vector3 LInCoordinatesTranslatedToCenterOfMass = Vector3.zero;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private List<IForce> forceSources;


    public float RotationPowerMultiplyer;
    public float gasSensitivity;
    // Start is called before the first frame update
    void Start()
    {
        forceSources = new List<IForce>();
        InertiaTensor = new List<List<float>>();
        for (int i = 0; i < 3; i++)
        {
            InertiaTensor.Add(new List<float>());
            for (int j = 0; j < 3; j++)
            {
                InertiaTensor[i].Add(0);
                if (i == j)
                    InertiaTensor[i][j] = 2.5f;
            }
        }
        Debug.Log("Intertia tensor:");
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                Debug.Log("I" + i.ToString() + j.ToString() + "= " + InertiaTensor[i][j]);
        invertedInertiaTensor = InverseMatrix(InertiaTensor);
        //gravityForce = this.gameObject.GetComponent<GravityForce>();
        lastPosition = transform.position;
        generalLevel = 0;
        for (int i = 0; i < engines.Count; i++)
        {
            engineLevels.Add(0);
            forceSources.Add(engines[i]);
        }
        foreach (ResistanceForce f in resistanceForces)
            forceSources.Add(f);
        foreach (WingForce w in wings)
            forceSources.Add(w);
        forceSources.Add(gravityForce);
    }

    // Update is called once per frame
    void Update()
    {
        //float gas = Input.GetAxis("Mouse ScrollWheel") * gasSensitivity * Time.deltaTime;
        //generalLevel += gas;
        if (Input.GetKey(KeyCode.W))
            generalLevel += generalLevelChangingSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            generalLevel -= generalLevelChangingSpeed * Time.deltaTime;
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < 0)
            generalLevel = 0;
        for (int i = 0; i < engineLevels.Count; i++)
            engineLevels[i] = generalLevel;
        //if (Input.GetKey(KeyCode.W))
        //{
        //    engineLevels[0] = generalLevel * RotationPowerMultiplyer;
        //    engineLevels[1] = generalLevel * RotationPowerMultiplyer;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    engineLevels[2] = generalLevel * RotationPowerMultiplyer;
        //    engineLevels[3] = generalLevel * RotationPowerMultiplyer;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    engineLevels[0] = generalLevel * RotationPowerMultiplyer;
        //    engineLevels[3] = generalLevel * RotationPowerMultiplyer;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    engineLevels[1] = generalLevel * RotationPowerMultiplyer;
        //    engineLevels[2] = generalLevel * RotationPowerMultiplyer;
        //}
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    engineLevels[1] = generalLevel * RotationPowerMultiplyer;
        //    engineLevels[3] = generalLevel * RotationPowerMultiplyer;
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    engineLevels[0] = generalLevel * RotationPowerMultiplyer;
        //    engineLevels[2] = generalLevel * RotationPowerMultiplyer;
        //}
        for (int i = 0; i < engineLevels.Count; i++)
            engines[i].Level = engineLevels[i];
    }
    private void LateUpdate()
    {
        ForceToCenterOfMass = Vector3.zero;
        MomentInCoordinatesTranslatedToCenterOfMass = Vector3.zero;

        List<Vector3> CurrentForceVectors;
        List<Vector3> AbsolutePointsOfForceApplying;
        foreach (IForce force in forceSources)
        {
            force.CountForce(out CurrentForceVectors, out AbsolutePointsOfForceApplying);
            for (int i = 0; i < CurrentForceVectors.Count; i++)
            {
                AddForce(CurrentForceVectors[i], AbsolutePointsOfForceApplying[i]);
                Debug.DrawLine(AbsolutePointsOfForceApplying[i], AbsolutePointsOfForceApplying[i] + CurrentForceVectors[i], Color.red);
            }

        }
        CharacterController c = new CharacterController();
        //Engine forces
        //for (int i = 0; i < engines.Count; i++)
        //{
        //    engines[i].CountForce();
        //    Debug.Log("Level:" + engines[i].Level);
        //    for(int j = 0; j < engines[i].CurrentForceVector.Count; j++)
        //    {
        //        AddForce(engines[i].CurrentForceVector[j], engines[i].AbsolutePointOfForceApplying[j]);
        //        Debug.DrawLine(engines[i].AbsolutePointOfForceApplying[j], engines[i].AbsolutePointOfForceApplying[j] + engines[i].CurrentForceVector[j], Color.red);
        //        if (i == 0 && j == 1)
        //        {
        //            Debug.Log("Rotation force: " + engines[i].CurrentForceVector[j]);
        //        }
        //    }

        //}
        //Gravity force
        //gravityForce.CountForce();
        //AddForce(gravityForce.CurrentForceVector[0], gravityForce.AbsolutePointOfForceApplying[0]);
        //Debug.DrawLine(gravityForce.AbsolutePointOfForceApplying[0], gravityForce.CurrentForceVector[0] + gravityForce.AbsolutePointOfForceApplying[0], Color.green);

        ////Resistance forces
        //for (int i = 0; i < resistanceForces.Count; i++)
        //{
        //    resistanceForces[i].CountForce();
        //    AddForce(resistanceForces[i].CurrentForceVector[0], resistanceForces[i].AbsolutePointOfForceApplying[0]);
        //    Debug.DrawLine(resistanceForces[i].AbsolutePointOfForceApplying[0]+new Vector3(0,0,0), resistanceForces[i].AbsolutePointOfForceApplying[0] + resistanceForces[i].CurrentForceVector[0], Color.blue);
        //    Debug.Log("Resistance force: " + resistanceForces[0]);
        //}
        //Rotaion dynamics
        float dt = Time.deltaTime;
        Vector3 dLInCoordinatedTranslatedToCenterOfMass = MomentInCoordinatesTranslatedToCenterOfMass * dt;
        LInCoordinatesTranslatedToCenterOfMass += dLInCoordinatedTranslatedToCenterOfMass;
        //Debug.Log("Force: " + ForceToCenterOfMass);
        //Debug.Log("Moment: " + MomentInCoordinatesTranslatedToCenterOfMass);
       // Debug.DrawLine(transform.position, transform.position + LInCoordinatesTranslatedToCenterOfMass, Color.black, 10);
        //Debug.Log("relativeL: " + LInCoordinatesTranslatedToCenterOfMass);
        Vector3 LInLocalCoordinates = transform.InverseTransformDirection(LInCoordinatesTranslatedToCenterOfMass);
        // Vector3 relativeAngularVelocity = MultiplyTensorOnVector3(invertedInertiaTensor, relativeL);
        Vector3 AngularVelocityInLocalCoordinates = LInLocalCoordinates / 0.01f;
        AbsoluteAngularVelocity = transform.TransformDirection(AngularVelocityInLocalCoordinates);
        // Debug.DrawRay(transform.position, AbsoluteAngularVelocity, Color.white,10,true,);
        transform.Rotate(-AbsoluteAngularVelocity.normalized, AbsoluteAngularVelocity.magnitude * dt, Space.World);


        //Translation Kinematics
        Vector3 acceleration = ForceToCenterOfMass / gravityForce.mass;
        Vector3 dv = acceleration * dt;
        velocityOfCenterMass += dv;
        transform.Translate(velocityOfCenterMass * dt, Space.World);
        //if (transform.position.y < 0)
        //{
       //     Vector3 newPosition = new Vector3(transform.position.x, 0, transform.position.z);
        //    transform.position = newPosition;
        //}
        velocityOfCenterMass = (transform.position - lastPosition) / dt;
        lastPosition = transform.position;


        //reverseI = InverseMatrix(InertiaTensor)
        //angularVelocity = 
        //Vector3 Angle = angularVelocity * dt;
    }
    private void AddForce(Vector3 forceInWorldCoordinates, Vector3 pointOfApplicationINWorldCoordinates)
    {
        ForceToCenterOfMass += forceInWorldCoordinates;
        Vector3 r = pointOfApplicationINWorldCoordinates - transform.position;
        //Vector3 r = pointOfApplicationINWorldCoordinates;
        Vector3 dM = -Vector3.Cross(r, forceInWorldCoordinates);
        MomentInCoordinatesTranslatedToCenterOfMass += dM;
        //Debug.DrawLine(pointOfApplicationINWorldCoordinates, pointOfApplicationINWorldCoordinates + forceInWorldCoordinates, Color.blue);
    }
    private List<List<float>> InverseMatrix(List<List<float>> matrix)
    {
        List<List<float>> inversedMatrix = new List<List<float>>();
        float matrixOder = matrix.Count;
        for (int i = 0; i < matrixOder; i++)
        {
            List<float> listOfFloats = new List<float>();
            for (int j = 0; j < matrixOder; j++)
            {
                listOfFloats.Add(0);
            }
            inversedMatrix.Add(listOfFloats);
        }

        float determinant = ReturnDeterminant(matrix);
        if (determinant == 0)
            return inversedMatrix;
        for (int i = 1; i <= matrixOder; i++)
            for (int j = 1; j <= matrixOder; j++)
            {
                float algebricAddition = AlgebricAddition(matrix, j, i);
                inversedMatrix[i - 1][j - 1] = algebricAddition / determinant;
            }
        return inversedMatrix;
    }
    private float AlgebricAddition(List<List<float>> matrix, float rowFromOne, float columnFromOne)
    {
        float matrixOder = matrix.Count;
        List<List<float>> minor = new List<List<float>>();
        for (int i = 0; i < matrixOder; i++)
        {
            List<float> listOfFloats = new List<float>();
            bool addRowToMinor = false;
            for (int j = 0; j < matrixOder; j++)
                if (j != columnFromOne - 1 && i != rowFromOne - 1)
                {
                    listOfFloats.Add(matrix[i][j]);
                    addRowToMinor = true;
                }
            if (addRowToMinor)
                minor.Add(listOfFloats);
        }
        float algebricAddition;
        if (minor.Count != 0)
            algebricAddition = (float)Mathf.Pow(-1, rowFromOne + columnFromOne) * ReturnDeterminant(minor);
        else
            algebricAddition = 1;
        return algebricAddition;
    }
    private Vector3 MultiplyTensorOnVector3(List<List<float>> tensor, Vector3 vector)
    {
        float x = 0;
        float y = 0;
        float z = 0;
        x = tensor[0][0] * vector.x + tensor[0][1] * vector.y + tensor[0][2] * vector.z;
        y = tensor[1][0] * vector.x + tensor[1][1] * vector.y + tensor[1][2] * vector.z;
        z = tensor[2][0] * vector.x + tensor[2][1] * vector.y + tensor[2][2] * vector.z;
        return new Vector3(x, y, z);
    }
    private float ReturnDeterminant(List<List<float>> matrix)
    {
        int matrixOder = matrix.Count;
        //Раскладываем по строке:
        float determinant = 0;
        for (int s = 0; s < matrixOder; s++)
        {
            //List<List<float>> minor = new List<List<float>>();
            //for (int i = 1; i < matrixOder; i++)
            //{
            //    List<float> listOfFloats = new List<float>();
            //    for (int j = 0; j < matrixOder; j++)
            //        if (j != s)
            //            listOfFloats.Add(matrix[i][j]);
            //    minor.Add(listOfFloats);
            //}
            //float algebricAddition;
            //if (minor.Count != 0)
            //    algebricAddition = (float)Math.Pow(-1, s) * ReturnDeterminant(minor);
            //else
            //    algebricAddition = 1;
            float algebricAddition = AlgebricAddition(matrix, 1, s + 1);
            determinant += matrix[0][s] * algebricAddition;
        }
        return determinant;
    }
}
