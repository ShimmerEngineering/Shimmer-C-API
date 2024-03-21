# Passkey Configuration App

This App allows you to set the passkey options for bonding/pairing with the Verisense device. There are three options
- No Passkey
- Defauly Passkey
- Clinical Passkey

The API has a fully configurable advertising ids, passkey ids, and passkey values, but for the purpose of limiting user error, the App is limited to the above. It will not be Shimmers responsibility should you use a configuration that ends up bricking the Verisense sensor. 

The following 
[Video Demo](https://user-images.githubusercontent.com/2862032/152300234-737ad996-80f8-4b37-9b13-a34c420e3137.mp4)
is an example of changing the sensor to default passkey from clinical passkey, note that the sensor has to be paired prior. Once the settings have been changed remember to power cycle and unpair --> pair the device.
We are defaulting to the following for the advertising ID, and we would recommend customers do the same. In particular paying attention to the Passkey ID definitions below

Verisense-00-XXXXXXXXXXXX : No Passkey

Verisense-01-XXXXXXXXXXXX : Default Passkey (123456)

Verisense-XXXXXXXXXXXX : Clinical Passkey 

Verisense-02-XXXXXXXXXXXX : Clinical Passkey (reserved for future if required)

The Clinical Passkey can be retrieved by contacting Shimmer if required.

_Note: Scanning can take a moment, and there are currently no UI indicators that it is indeed scanning_

## Requirements
Verisense FW Version >=1.2.99, using an incorrect version will lead to the following error as shown here in the
[Video Demo](https://user-images.githubusercontent.com/2862032/152299942-ee2135d3-2854-4e19-bd4a-365e66508d72.mp4)
If you require updating the firmware on your Verisense device, please contact Shimmer.

