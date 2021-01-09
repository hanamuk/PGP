package sample;

public class st {
	
private int x, y;
		st(int x1, int y1)
		{
			this.x = x1;
			this.y= y1;
			
		}
		public void show(){
			
			System.out.printf("%d + %d = %d", x,y,x+y);
		}
		
		public int getX()
		{
			return x;
			
		}
		
		public int getY()
		{
			return y;
			
		}
}
