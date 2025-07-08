# Passkey Configuration App

This App allows you to set the passkey options for bonding/pairing with the Verisense device. There are three options
- No Passkey
- Default Passkey
- Clinical Passkey

The API has a fully configurable advertising ids, passkey ids, and passkey values, but for the purpose of limiting user error, the App is limited to the above. It will not be Shimmers responsibility should you use a configuration that ends up bricking the Verisense sensor. 

The following steps are how to configure the device
1) Pair the Verisense device (if required)
2) Scan
3) Pick Verisense device from the list of scanned devices
4) Connect
5) Select the passkey configuration from the list of choices
6) Click Write Passkey
7) Disconnect
8) Power cycle the Verisense device and unpair it

is an example of changing the sensor to default passkey from clinical passkey, note that the sensor has to be paired prior. Once the settings have been changed remember to power cycle and unpair --> pair the device.
We are defaulting to the following for the advertising ID, and we would recommend customers do the same. In particular paying attention to the Passkey ID definitions below

Verisense-00-XXXXXXXXXXXX : No Passkey

Verisense-01-XXXXXXXXXXXX : Default Passkey (123456)

Verisense-XXXXXXXXXXXX : Clinical Passkey 

Verisense-02-XXXXXXXXXXXX : Clinical Passkey (reserved for future if required)

The Clinical Passkey can be retrieved by contacting Shimmer if required.

_Note: Scanning can take a moment, and there are currently no UI indicators that it is indeed scanning_

## Requirements
Verisense FW Version >=1.2.99, using an incorrect version will lead to an error being displayed.
If you require updating the firmware on your Verisense device, please contact Shimmer.

