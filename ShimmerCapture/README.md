# Shimmer-C-API

# REV0_3
- Major update to ecgtohr algorithm and filtering algorithm, user should see major improvements in both ecgtohr and ppgtohr algorithms.
- Currently uses ShimmerClosedLibraryRev0_2

BUILDING THE LIBRARY NOTES
- Switch output type to Windows Application
- Change Assembly name accordingly ShimmerCapture v0.X
- Update rev in assemblyinfo.cs (follow rev above)
- Update ShimmerClosedLibrary, remove old dll from libs folder and replace with new, update the reference
