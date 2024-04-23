**JUMPKING ONLINE WITH UNITY** 

1. **Giới<a name="_page2_x97.00_y223.00"></a> thiệu về trò chơi** 
- **Nhân vật:** King và Knight.** 

![](MD/Aspose.Words.0f2264f7-57f5-4ecd-bf6d-fd0698379140.003.png)

*Figure 1: Knight character* 

![](MD/Aspose.Words.0f2264f7-57f5-4ecd-bf6d-fd0698379140.004.png)

*Figure 2: King character* 

- **Bản đồ:** Redcrow Woods.** 

![](MD/Aspose.Words.0f2264f7-57f5-4ecd-bf6d-fd0698379140.005.jpeg)

*Figure 3: Redcrow Woods map* 

2. **Gameplay<a name="_page3_x97.00_y363.00"></a>**  
- Số lượng người chơi trong một phòng: 2. 
- Sau khi nhập mã phòng, người chơi sẽ được đưa vào lobby, host sẽ có quyền bắt đầu trò chơi. 
- Khi vào game, người chơi sẽ được spawn vào bản đồ game. 
- Bản đồ sẽ bao gồm:  
  - Block. 
  - Building object. 
  - Npc. 
- Người chơi nào lên được đỉnh tháp trước sẽ là người chiến thắng. 
2. **Triển<a name="_page3_x97.00_y535.00"></a> khai** 
1. **Tính<a name="_page3_x97.00_y556.00"></a> năng** 
- Tạo phòng chơi với số lượng người chơi là 2 (1 host). 
- Tham gia phòng chơi bằng code (1 client). 
- Tạo lobby, relay.  
- Tạm dừng game. 

<a name="_page4_x97.00_y71.00"></a>**Một số hình ảnh minh họa:**  

![](MD/Aspose.Words.0f2264f7-57f5-4ecd-bf6d-fd0698379140.006.jpeg)

*Figure 4: Create and Join game room* 

![](MD/Aspose.Words.0f2264f7-57f5-4ecd-bf6d-fd0698379140.007.jpeg)

*Figure 5: Game lobby* 

![](MD/Aspose.Words.0f2264f7-57f5-4ecd-bf6d-fd0698379140.008.jpeg)

*Figure 6: Pause game* 

2. **Mô<a name="_page5_x97.00_y354.00"></a> hình phân rã chức năng (BFD)** 

![](MD/ltm-sodo.png)

*Figure 7: Game BFD* 

3. **Cấu<a name="_page6_x97.00_y71.00"></a> trúc gói tin** 
- **Thư viện:** *Unity Netcode*, *Unity Relay*, *Unity Lobby* 
- **Chức năng:**  
- Đồng bộ vị trí *Player*.** 
- Đồng bộ *Animation*.** 
- Tạo kết nối.** 
- Tạo phòng chờ.** 
- Kết nối *Player* vào *Server* thông qua *Unity Services* (Global Hosting).** 
1. **Client<a name="_page6_x97.00_y227.00"></a> đến Server** 



|**Control Message:**  Connect/Disconnect/Data/ |**Length:** 6144 bytes |
| :- | - |
|**Sender:** Client** |**Receiver:** Server** |
|**Body** ||



|**Packet** |||||
| - | :- | :- | :- | :- |
|NetworkObjectId |Position |Rotation |Scale |State |

*Source: NetworkObject.cs* 

- **Giải nghĩa**: 
- *Connect*: người chơi yêu cầu kết nối vào Server. 
- *Disconnect*: người chơi yêu cầu rời Server. 
- *Data*: người chơi gửi thông tin trong Packet. 
- *NetworkObjectId*: Id của người chơi. 
- *Position*: Vị trí người chơi hiện tại. 
- *Rotation*: Trạng thái xoay của người chơi, cố định giá trị (0,0,0). 
- *Scale*: Trạng thái lật của người chơi, di chuyển sang phải (1,1,1), di chuyển sang phải (-1,1,1). 
- *State*: hoạt ảnh hiện tại, gồm 5 hoạt ảnh {idle, run, hold, jump, fall}. 
2. **Server<a name="_page7_x97.00_y106.00"></a> đến Client**  



|**Control Message:**  IsServer/IsClient/IsListening/ IsApproved |**Length:** 6144 bytes |
| :-: | - |
|**Sender:** Server |**Receiver:** Client |
|**Body** ||



|**Packet** ||||
| - | :- | :- | :- |
|LocalClientId |NetworkPrefabs |NetworkTransform |NetworkAnimator |

- **Giải nghĩa:** 
- *IsServer*: kiểm tra xem có phải là Server. 
- *IsClient*: kiểm tra nếu là người chơi. 
  - Trong đồ án này, Host sẽ là Server đồng thời cũng là Client. 
- *IsListening*: kiểm tra xem Server có đang mở kết nối. 
- *IsApproved*: kiểm tra người chơi có được chấp nhận. 
- *LocalClientId*: Id của người chơi, được cấp bởi Server thông qua NetworkObjectId. 
- *NetworkPrefabs*: Nhân vật của người chơi. 
- *NetworkTransform*: Vị trí người chơi được trả về, bao gồm: 
  - *Position* 
  - *Rotation* 
  - *Scale* 
- *NetworkAnimator*: Hoạt ảnh nhân vật được trả về, bao gồm: 
  - *Sprite* 
  - *Animation* 
- Đối với mỗi lần gửi, Server sẽ gửi cho (n – 1) gói tin cho (n) người chơi.  
4. **Network<a name="_page8_x97.00_y71.00"></a> stack**  

***(Xem ở trang kế tiếp …)*** 

![](MD/ltm-sodo2.png)

3. **Phân<a name="_page10_x97.00_y71.00"></a> chia công việc** 



|**MSSV** |**Họ tên** |**Công việc** |**% Công việc** |
| - | - | - | - |
|21522797 |Lê Huỳnh Quang Vũ |<p>- Thiết kế gameplay, lập trình logic game. </p><p>- Thiết kế mạng, lập trình mạng. </p><p>- Thuyết trình. </p><p>- Làm báo báo. </p>|100% |

***Video demo*:[ https://drive.google.com/file/d/1auDn2UTejUFmqPWJiGwy- PbYFSz_Qs_t/view?usp=sharing ](https://drive.google.com/file/d/1auDn2UTejUFmqPWJiGwy-PbYFSz_Qs_t/view?usp=sharing)**