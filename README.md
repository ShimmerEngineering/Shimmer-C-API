# Shimmer-C-API Rev0.7 (BETA)
The C# APi is currently in a BETA development state, users are free to use and provide feedback. For users working on production code we recommend downloading the API from the Shimmer Website http://www.shimmersensing.com/support/wireless-sensor-networks-download/

To use, build the ShimmerAPI project and make sure the examples (e.g. ShimmerCapture) references the build dll.

NOTE: Where require the dlls for ShimmerLibrary, InTheHand, MathNet.Numerics can be found in the libraries folder in the ShimmerAPI project

Changes since Rev0.6
- Shimmer and Shimmer32Feet have been converted to abstract classes, in the past these classes supported BTStream while ShimmerSDBT supported LogandStream. BTStream has since been deprecated


