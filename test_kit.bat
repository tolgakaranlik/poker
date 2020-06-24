echo Sunucu çalıştırılıyor...
@start T1GameRoomServer\bin\debug\T1GameRoomServer.exe -casinoid=4

echo Test istemcisi çalıştırılıyor...
@cd T1TestClient\bin\debug
@start T1TestClient.exe

echo Tamamlandı