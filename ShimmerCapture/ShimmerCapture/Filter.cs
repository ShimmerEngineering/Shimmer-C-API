using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

/** 
 *
 This is a BlackMan-Windowed-Sinc Filter. Algorithm for calculating 
 filter coefficients from "The Scientist and Engineer's Guide to Digital Signal Processing",
 copyright ©1997-1998 by Steven W. Smith. 
 For more information visit the book's website at: www.DSPguide.com.
 *
 * Rev0.4
 * - Updated to coefficients[(nTaps/2)] = coefficients[(nTaps/2)] +1; 
 * 
 * Rev0.3
 *  
 * Rev 0.2
 *  
 * Changes since 0.1
 * - Handle an error case - when array of data to filter (number of samples) is bigger than the array of data to filter from the previous iteration
 * - Added resetBuffers() function 
 * 
 */

namespace ShimmerAPI
{
    public class Filter
    {
	
	public static int LOW_PASS = 0;
    public static int HIGH_PASS = 1;
    public static int BAND_PASS = 2;
    public static int BAND_STOP = 3;

    // filter parameters   
    private int filterType;
    private double samplingRate = Double.NaN;
    private double[] cornerFrequency;
    private int nTaps;
    private double minCornerFrequency, maxCornerFrequency;

    // buffered data (for filtering streamed data)
    private double[] bufferedX;

    // filter coefficients {h}
    private double[] coefficients;

    // input parameters are invalid
    private bool validparameters = false;

    // default parameters
    private double defaultSamplingRate = 512;
    private double[] defaultCornerFrequency = { 0.5 };
    private int defaultNTaps = 200;
    
    public Filter() {
    	filterType=LOW_PASS;

    	SetFilterParameters(LOW_PASS, defaultSamplingRate, defaultCornerFrequency, defaultNTaps);
    }
     
    public Filter(int filterType) {
    	
    	this.filterType=filterType;

    	SetFilterParameters(filterType, defaultSamplingRate, defaultCornerFrequency, defaultNTaps);
    }
    
    public Filter(int filterType, double samplingRate, double[] cornerFrequency) {
    	
    	this.filterType=filterType;
    	
        SetFilterParameters(filterType, samplingRate, cornerFrequency, defaultNTaps);
    }

    public Filter(int filterType, double samplingRate, double[] cornerFrequency, int nTaps) {
    	
    	this.filterType=filterType;
    	
        SetFilterParameters(filterType, samplingRate, cornerFrequency, nTaps);
    }
    
    
    public void SetFilterParameters(int LoHi, double samplingRate, double[] cornerFrequency, int nTaps) {
    	
    	//reset the buffers
    	this.bufferedX = null; 
    	
    	if(cornerFrequency.Length!=1){
    		if(cornerFrequency[0] > cornerFrequency[1]){
    			minCornerFrequency = cornerFrequency[1];
    			maxCornerFrequency = cornerFrequency[0];
    		}
    		else{
    			minCornerFrequency = cornerFrequency[0];
    			maxCornerFrequency = cornerFrequency[1];
    		}
    	}
    	else
    		minCornerFrequency = maxCornerFrequency = cornerFrequency[0];
    	
    	
    	if (maxCornerFrequency > samplingRate / 2)
        {
            this.validparameters = false;
            throw new Exception("Error: cornerFrequency is greater than Nyquist frequency. Please choose valid parameters.");
        }
        else
        {
            if (nTaps % 2 != 0)
            {
                nTaps--;
                //JOptionPane.showMessageDialog(null, "Warning: nPoles is not an even number. nPoles will be rounded to " +Integer.toString(nTaps));
            }

            if (LoHi == LOW_PASS || LoHi == HIGH_PASS) // High pass or Low pass filter
            {
                this.samplingRate = samplingRate;
                this.cornerFrequency = cornerFrequency;
                this.nTaps = nTaps;

                double fc = (cornerFrequency[0] / samplingRate);
                // calculate filter coefficients
                coefficients = new double[nTaps];
                coefficients = calculateCoefficients(fc, LoHi, nTaps);
                this.validparameters = true;
            }
            else if (LoHi == BAND_PASS || LoHi == BAND_STOP)
            {
                if (cornerFrequency.Length != 2)
                	throw new Exception("Error. Bandpass or bandstop filter requires two corner frequencies to be specified");
                
                this.samplingRate = samplingRate;
                this.nTaps = nTaps;
                
                double fcHigh = maxCornerFrequency / samplingRate;
                double fcLow = minCornerFrequency / samplingRate;

                // calculate filter coefficients
                double[] coefficientHighPass = calculateCoefficients(fcHigh, HIGH_PASS, nTaps);
                double[] coefficientLowPass = calculateCoefficients(fcLow, LOW_PASS, nTaps);
                
                coefficients = new double[coefficientHighPass.Length];
                for(int i=0; i<coefficientHighPass.Length;i++){
                	if(LoHi == BAND_PASS)
                		coefficients[i] = - (coefficientHighPass[i] + coefficientLowPass[i]); //sum of HPF and LPF for bandstop filter, spectral inversion for bandpass filter
                	else
                		coefficients[i] = coefficientHighPass[i] + coefficientLowPass[i]; //sum of HPF and LPF for bandstop filter
                }
                
                if(LoHi == BAND_PASS){
                	//coefficients[(nTaps/2)+1] = coefficients[(nTaps/2)+1] +1;
                    coefficients[(nTaps / 2)] = coefficients[(nTaps / 2)] + 1;
                }
                
                this.validparameters = true;
            }
            else
            	throw new Exception("Error. Undefined filter type: use 0 - lowpass, 1 - highpass, 2- bandpass, or 3- bandstop");
        }
    }
    
        // Note: start is inclusive, end is exclusive (as is conventional
        // in computer science)
        public static void Fill<T>(T[] array, int start, int end, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (start < 0 || start >= end)
            {
                throw new ArgumentOutOfRangeException("fromIndex");
            }
            if (end > array.Length)
            {
                throw new ArgumentOutOfRangeException("toIndex");
            }
            for (int i = start; i < end; i++)
            {
                array[i] = value;
            }
        }

    public double filterData(double data) 
    {
    	double dataFiltered = Double.NaN;
    	
    	 if (!this.validparameters)
         	throw new Exception("Error. Filter parameters are invalid. Please set filter parameters before filtering data.");
         else
         {
        	 int nSamples = 1;
             int bufferSize = this.nTaps; 
        	 if(bufferedX==null){
        		 bufferedX = new double[bufferSize+nSamples]; // buffers are initiliazed to 0 by default
        		 //Arrays.fill(bufferedX, data); // fill the buffer X with the first data        		 
                 Fill(bufferedX,0,bufferedX.Length,data);
        	 }
        	 else{
        		 //System.arraycopy(bufferedX, 1, bufferedX, 0, bufferedX.Length-1); //all the elements in the buffer are shifted one position to the left
                 Array.Copy(bufferedX, 1, bufferedX, 0, bufferedX.Length-1);
        		 bufferedX[bufferedX.Length-1] = data;
        	 }
        	 
        	 double Y = filter(bufferedX);
        	 dataFiltered = Y;
        	 
         }
    	
    	return dataFiltered;    
    }
    
    public double[] filterData(double[] data)
    {
    	if (!this.validparameters)
         	throw new Exception("Error. Filter parameters are invalid. Please set filter parameters before filtering data.");
         else
         {
        	 double[] dataFiltered = new double[data.Length];
        	 
        	 for(int i=0; i<data.Length; i++){
        		 double individualDataFiltered = filterData(data[i]);
        		 dataFiltered[i] = individualDataFiltered;
        	 }
        	 
        	 return dataFiltered;
         }
    }
    
    private double filter(double[] X)
    {
    	
    	int nTaps = coefficients.Length;
    	double Y = 0;
    	
    	for(int i=0; i<nTaps; i++)
    		Y += X[nTaps-i]*coefficients[i];
    	
    	return Y;
    }
    
    private double[] calculateCoefficients(double fc, int LoHi, int nTaps) 
    {
        if (!(LoHi == LOW_PASS || LoHi == HIGH_PASS))
        	throw new Exception("Error: the function calculateCoefficients() can only be called for LPF or HPF.");

        //Initialization
        int M = nTaps;
        double[] h = new double[M];
        for(int i=0;i<M; i++)
        	h[i] = 0;
        
        for(int i=0;i<M;i++){
        	h[i] = 0.42 - 0.5 * Math.Cos((2*Math.PI*i)/M) + 0.08*Math.Cos((4*Math.PI*i)/M);
        	if(i!=M/2)
        		h[i] = h[i] * (Math.Sin(2*Math.PI*fc*(i-(M/2))))/(i-(M/2));
        	else
        		h[i] = h[i] * (2*Math.PI*fc);
        }
         
        double gain = 0;
        for(int i=0;i<h.Length;i++)
        	gain += h[i];
        
        for(int i=0;i<h.Length;i++){
        	if(LoHi == HIGH_PASS){
        		h[i] = - h[i]/gain;
        	}
        	else
        		h[i] = h[i]/gain;
        }

        if(LoHi == HIGH_PASS){
        	h[M/2] = h[M/2] + 1;
        }
        
        return h;
    }
    
    
    public double GetSamplingRate(){
        return samplingRate;
    }
    
    protected void SamplingRate(double samplingrate){
        samplingRate = samplingrate;
    }
    
    
    public double[] GetCornerFrequency(){
        return cornerFrequency;
    }
    
    protected void SetCornerFrequency(double[] cf){
        cornerFrequency = cf;
    }
    
    public static double[] fromListToArray(List<Double> list){
		
		double [] array = new double[list.Count];
		for(int i=0;i<list.Count;i++)
			array[i] = list[i];
		
		return array;
	}
    
    public void resetBuffers(){
    	
    	bufferedX = null;
    }
    
	public int getFilterType() {
		return filterType;
	}    
    

    }

}
