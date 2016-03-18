using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3;
	[SerializeField]
	private float thrusterForce = 1000f;

	[Header("Spring Settings:")]
	[SerializeField]
	private JointDriveMode jointMode = JointDriveMode.Position;
	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;


	private PlayerMotor moter;
	private ConfigurableJoint joint;

	void Start(){
		moter = GetComponent<PlayerMotor> ();
		joint = GetComponent<ConfigurableJoint> ();

		SetJointSettings (jointSpring);
	}

	void Update(){
		//Calculate Movement as a 3D vector
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw ("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		//final movement vector
		Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;
	
		//Apply movement
		moter.Move(_velocity);

		//Calculate rotation as 3D vector(turning around)
		float _yRot = Input.GetAxisRaw("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f)*lookSensitivity;

		//Aplly rotation
		moter.Rotate(_rotation);

		//Calculate camera rotation as 3D vector
		float _xRot = Input.GetAxisRaw("Mouse Y");

		float _cameraRotationX =_xRot*lookSensitivity;

		//Aplly camerarotation
		moter.RotateCamera(_cameraRotationX);

		//Calculate thruster based on player
		Vector3 _thresterFource = Vector3.zero;

		if (Input.GetButton ("Jump")) {
			_thresterFource = Vector3.up * thrusterForce;
			SetJointSettings (0f);
		} else {
			SetJointSettings (jointSpring);
		}

		//Apply the thruster force
		moter.ApplyThruster(_thresterFource);
	}

	private void SetJointSettings(float _jointSpring)
	{
		joint.yDrive = new JointDrive
		{
			mode = jointMode, 
			positionSpring = jointSpring, 
			maximumForce = jointMaxForce
		};
	}
}
