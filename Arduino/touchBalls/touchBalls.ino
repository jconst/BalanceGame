#include <ps2.h>

#define yHigh 14
#define xHigh 15
#define yLow  16
#define xLow  17

PS2 mouse[3] = {PS2(2, 3), PS2(5, 6), PS2(7, 8)}; //(clock, data)

void setup()
{
  Serial.begin(57600);
  Serial.println("1");
  mouse_init(0);
  mouse_init(1);
  mouse_init(2);
}

void loop()
{
  poll_mouse(0);
  poll_mouse(1);
  poll_mouse(2);
  readTouch();
}

/*
 * initialize the mouse. Reset it, and place it into remote
 * mode, so we can get the encoder data on demand.
 */
void mouse_init(int i)
{
  Serial.println("2");
  mouse[i].write(0xff);  // reset
  Serial.println("3");
  mouse[i].read();  // ack byte
  mouse[i].read();  // blank */
  mouse[i].read();  // blank */
  mouse[i].write(0xf0);  // remote mode
  mouse[i].read();  // ack
  delayMicroseconds(100);
}

void poll_mouse(int i)
{
  /* get a reading from the mouse */
  mouse[i].write(0xeb);  // give me data!
  mouse[i].read();      // ignore ack
  char mstat = mouse[i].read();
  char mx = mouse[i].read();
  char my = mouse[i].read();

  /* send the data to the Mac */
  Serial.print(":m");
  Serial.print((int)i);
  Serial.print("\t");
  Serial.print((int)mstat);
  Serial.print("\t");
  Serial.print((int)mx);
  Serial.print("\t");
  Serial.print((int)my);
  Serial.println();
  delay(20);
}

void readTouch() {
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
  delay(10);
}
