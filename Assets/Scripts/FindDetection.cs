using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;
using UnityEngine.UI;

public class FindDetection : MonoBehaviour
{
    public ARFaceManager afm;
    public GameObject smallCube;
    public Text vertexIndex;

    List<GameObject> faceCubes = new List<GameObject>();
    ARCoreFaceSubsystem subSys;
    NativeArray<ARCoreFaceRegionData> regionData;

    void Start()
    {
        // 위치 표시를 위한 작은 큐브 3개를 생성한다.
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(smallCube);
            faceCubes.Add(go);
            go.SetActive(false);
        }

        // AR Face Manager가 얼굴을 인식할 때 실행할 함수를 연결한다.
        //afm.facesChanged += OnDetectThreePoints;
        afm.facesChanged += OnDetectFaceAll;

        // AR Foundation의 XRFaceSubsystem 클래스 변수를 AR Core의 ARCoreFaceSubsystem 클래스 변수로 캐스팅한다.
        subSys = (ARCoreFaceSubsystem)afm.subsystem;
    }

    // facesChanged 델리게이트(Action)에 연결할 함수
    void OnDetectThreePoints(ARFacesChangedEventArgs args)
    {
        // 얼굴 인식 정보가 갱신된 것이 있다면...
        if (args.updated.Count > 0)
        {
            // 인식된 얼굴에서 특정 위치를 가져온다.
            subSys.GetRegionPoses(args.updated[0].trackableId, Allocator.Persistent, ref regionData);

            // 인식된 얼굴의 특정 위치(0: 코 끝, 1: 이마 좌측, 2: 이마 우측)에 오브젝트를 위치한다.
            for (int i = 0; i < regionData.Length; i++)
            {
                faceCubes[i].transform.position = regionData[i].pose.position;
                faceCubes[i].transform.rotation = regionData[i].pose.rotation;
                faceCubes[i].SetActive(true);
            }
        }
        // 얼굴 인식 정보를 잃었다면...
        else if (args.removed.Count > 0)
        {
            // 오브젝트를 비활성화 한다.
            for (int i = 0; i < regionData.Length; i++)
            {
                faceCubes[i].SetActive(false);
            }
        }
    }

    // 얼굴 메쉬 데이터를 이용한 방식
    void OnDetectFaceAll(ARFacesChangedEventArgs args)
    {
        // 얼굴을 인식하였을 때에는...
        if (args.updated.Count > 0)
        {
            // 텍스트 UI에 적힌 문자열 데이터를 정수형 데이터로 변환한다.  
            int num = int.Parse(vertexIndex.text);

            // 얼굴 정점 배열에서 지정한 인덱스에 해당하는 좌표를 가져온다.
            //Vector3 vertPosition = args.updated[0].vertices[100];
            Vector3 vertPosition = args.updated[0].vertices[num];
            
            // 정점 좌표를 월드 좌표로 변환한다.
            vertPosition = args.updated[0].transform.TransformPoint(vertPosition);

            // 준비된 큐브 하나를 활성화하고 정점 위치에 가져다 놓는다.
            faceCubes[0].SetActive(true);
            faceCubes[0].transform.position = vertPosition;
            //faceCubes[0].transform.rotation = Quaternion.LookRotation(args.updated[0].normals[num]);
        }
        // 얼굴을 인식하지 못했을 때에는...
        else if (args.removed.Count > 0)
        {
            faceCubes[0].SetActive(false);
        }
    }
}
