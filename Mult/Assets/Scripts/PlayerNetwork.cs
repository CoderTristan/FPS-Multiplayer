using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetwork : NetworkBehaviour
{

    [SerializeField] private Transform spawnedObjectPrefab;
    private Transform spawnedTrans;
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData
    {
        _int = 56,
        _bool = true,
        message = "Initial Message"
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
{
    serializer.SerializeValue(ref _int);
    serializer.SerializeValue(ref _bool);
    serializer.SerializeValue(ref message);

}

    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
        Debug.Log(OwnerClientId + " " + newValue._int + " " + newValue._bool + " " + newValue.message);
        };
    }


    private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            spawnedTrans = Instantiate(spawnedObjectPrefab);
            spawnedTrans.GetComponent<NetworkObject>().Spawn();
            Debug.Log("Spawned Object");

            //TestServerRpc();
            /*
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "Hello World"
            };
            */
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnedTrans.gameObject);
        }
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W)) moveDir.z += 1f;
        if (Input.GetKeyDown(KeyCode.S)) moveDir.z -= 1f;
        if (Input.GetKeyDown(KeyCode.A)) moveDir.x -= 1f;
        if (Input.GetKeyDown(KeyCode.D)) moveDir.x += 1f;
        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

    }

    [ServerRpc]
    private void TestServerRpc()
    {
        Debug.Log("Testsr" + OwnerClientId);
    }
}
