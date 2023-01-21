using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
    // Start is called before the first frame update
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
    List<ResistanceForce> resistanceForces;

    private Vector3 ForceToCenterOfMass;
    private Vector3 AbsoluteMomentCenterOfMass;

    private Vector3 velocityOfCenterMass = Vector3.zero;
    private Vector3 AbsoluteAngularVelocity = Vector3.zero;
    private Vector3 AbsoluteL = Vector3.zero;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    // Start is called before the first frame update
    void Start()
    {
        InertiaTensor = new List<List<float>>();
        for(int i = 0; i < 3; i++)
        {
            InertiaTensor.Add(new List<float>());
            for (int j = 0; j < 3; j++)
            {
                InertiaTensor[i].Add(0);
                if (i == j)
                    InertiaTensor[i][j] = 0.1f;
            }
        }
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                Debug.Log(InertiaTensor[i][j]);
        invertedInertiaTensor = InverseMatrix(InertiaTensor);
        //gravityForce = this.gameObject.GetComponent<GravityForce>();
        lastPosition = transform.position;
        generalLevel = 0;
        for (int i = 0; i < engines.Count; i++)
            engineLevels.Add(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.PageUp))
            generalLevel += generalLevelChangingSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.PageDown))
            generalLevel -= generalLevelChangingSpeed * Time.deltaTime;
        if (generalLevel > 1)
            generalLevel = 1;
        if (generalLevel < 0)
            generalLevel = 0;
        for (int i = 0; i < engineLevels.Count; i++)
            engineLevels[i] = generalLevel;
        foreach (EngineForce engine in engines)
            engine.Level = generalLevel;

        
    }
    private void LateUpdate()
    {
        //Physics counting
        ForceToCenterOfMass = Vector3.zero;
        AbsoluteMomentCenterOfMass = Vector3.zero;

        //Engine forces
        for (int i = 0; i < engines.Count; i++)
        {
            engines[i].CountForce();
            for(int j = 0; j < engines[i].CurrentForceVector.Count; j++)
            {
                AddForce(engines[i].CurrentForceVector[j], engines[i].AbsolutePointOfForceApplying[j]);
                Debug.DrawLine(engines[i].AbsolutePointOfForceApplying[j], engines[i].AbsolutePointOfForceApplying[j] + engines[i].CurrentForceVector[j], Color.red);
                if (i == 0 && j == 1)
                {
                    Debug.Log("Rotation force: " + engines[i].CurrentForceVector[j]);
                }
            }
            
        }
        //Gravity force
        gravityForce.CountForce();
        AddForce(gravityForce.CurrentForceVector[0], gravityForce.AbsolutePointOfForceApplying[0]);
        Debug.DrawLine(gravityForce.AbsolutePointOfForceApplying[0], gravityForce.CurrentForceVector[0] + gravityForce.AbsolutePointOfForceApplying[0], Color.green);
       
        //Resistance forces
        for (int i = 0; i < resistanceForces.Count; i++)
        {
            resistanceForces[i].CountForce();
            AddForce(resistanceForces[i].CurrentForceVector[0], resistanceForces[i].AbsolutePointOfForceApplying[0]);
            Debug.DrawLine(resistanceForces[i].AbsolutePointOfForceApplying[0]+new Vector3(0.01f,0,0), resistanceForces[i].AbsolutePointOfForceApplying[0] + resistanceForces[i].CurrentForceVector[0], Color.blue);
            Debug.Log("Resistance force: " + resistanceForces);
        }
        //Rotaion dynamics
        float dt = Time.deltaTime;
        Vector3 dL = AbsoluteMomentCenterOfMass * dt;
        AbsoluteL += dL;
        Debug.Log("Force: " + ForceToCenterOfMass);
        Debug.Log("AbsoluteM: " + AbsoluteMomentCenterOfMass);
        Vector3 transliationL = -gravityForce.mass * Vector3.Cross(transform.position, velocityOfCenterMass);
        Vector3 relativeL = transform.InverseTransformDirection(AbsoluteL-transliationL);
        Debug.DrawLine(transform.position, transform.position+AbsoluteL - transliationL, Color.black, 10);
        Debug.Log("relativeL: " +( AbsoluteL - transliationL));
        Vector3 relativeAngularVelocity = MultiplyTensorOnVector3(invertedInertiaTensor, relativeL);
        AbsoluteAngularVelocity = transform.TransformDirection(relativeAngularVelocity);
       // Debug.DrawRay(transform.position, AbsoluteAngularVelocity, Color.white,10,true,);
        transform.Rotate(-AbsoluteAngularVelocity.normalized, AbsoluteAngularVelocity.magnitude * dt, Space.World);


        //Translation Kinematics
        Vector3 acceleration = ForceToCenterOfMass / gravityForce.mass;
        Vector3 dv = acceleration * dt;
        velocityOfCenterMass += dv;
        //transform.Translate(velocityOfCenterMass * dt);
        if (transform.position.y < 0)
        {
            Vector3 newPosition = new Vector3(transform.position.x, 0, transform.position.z);
            transform.position = newPosition;
        }
        velocityOfCenterMass = (transform.position - lastPosition) / dt;
        lastPosition = transform.position;

        
        //reverseI = InverseMatrix(InertiaTensor)
        //angularVelocity = 
        //Vector3 Angle = angularVelocity * dt;
    }
    private void AddForce(Vector3 forceInWorldCoordinates, Vector3 pointOfApplicationINWorldCoordinates)
    {
        ForceToCenterOfMass += forceInWorldCoordinates;
        //Vector3 r = pointOfApplicationINWorldCoordinates - transform.position;
        Vector3 r = pointOfApplicationINWorldCoordinates;
        Vector3 dM = -Vector3.Cross(r, forceInWorldCoordinates);
        AbsoluteMomentCenterOfMass +=dM;
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
