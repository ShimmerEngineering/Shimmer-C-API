# Disconnect Test App
Purpose of the disconnect test app is to check if the Verisense device is disconnected successfully and if there is any issue arise while disconnecting. The app consists of four different tests each uses different methods to disconnect.

1. Test 1
   - connect
   - call Disconnect() in RadioPluginBLE
   - check that it is indeed disconnected
2. Test 2
   - connect
   - execute disconnect request
   - check that it is indeed disconnected
3. Test 3
   - connect
   - call Disconnect() in RadioPluginBLE
   - call Disconnect() in RadioPluginBLE
   - check that it is indeed disconnected
4. Test 4
   - connect
   - power off the device
   - execute disconnect request
   - check that it is indeed disconnected