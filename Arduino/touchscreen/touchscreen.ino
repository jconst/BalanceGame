// Taken from http://kousaku-kousaku.blogspot.com/2008/08/arduino_24.html
// and https://kalshagar.wikispaces.com/Arduino+and+a+Nintendo+DS+touch+screen
/*
#define xLow  14
#define xHigh 15
#define yLow  16
#define yHigh 17
*/
//modified to match my sparkfun connector
#define yHigh 14
#define xHigh 15
#define yLow  16
#define xLow  17
 
 
void setup(){
  Serial.begin(57600);
}
 
void loop(){
  pinMode(xLow,OUTPUT);
  pinMode(xHigh,OUTPUT);
  digitalWrite(xLow,LOW);
  digitalWrite(xHigh,HIGH);
 
  digitalWrite(yLow,LOW);
  digitalWrite(yHigh,LOW);
 
  pinMode(yLow,INPUT);
  pinMode(yHigh,INPUT);
  delay(10);
 
  //xLow has analog port -14 !!
  int x=analogRead(yLow -14);
 
  pinMode(yLow,OUTPUT);
  pinMode(yHigh,OUTPUT);
  digitalWrite(yLow,LOW);
  digitalWrite(yHigh,HIGH);
 
  digitalWrite(xLow,LOW);
  digitalWrite(xHigh,LOW);
 
  pinMode(xLow,INPUT);
  pinMode(xHigh,INPUT);
  delay(10);
 
  //xLow has analog port -14 !!
  int y=analogRead(xLow - 14);
 
  Serial.print(":touch\t");
  Serial.print(x,DEC); 
  Serial.print("\t");
  Serial.println(y,DEC);
 
  delay(100);
}