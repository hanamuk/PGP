package sample;
import java.io.File;
import java.lang.*;
import java.util.ArrayList;
import java.util.Iterator;


public class chw {

	public static void main(String[] args) {
		// TODO Auto-generated method stub

		StringBuffer sb = new StringBuffer("good");
		System.out.println(sb);
		sb.append("Morning");
		System.out.println(sb);
		
		st a = new st(1,10);
		st b = new st(3,4);
		
		ArrayList<st> stList = new ArrayList<st>();
		
		stList.add(a);
		stList.add(b);
		
		int size = stList.size();
		
		st c = stList.get(1);
		
		System.out.print(c.getX());
		
		Iterator<st> it = stList.iterator();
		while(it.hasNext()){
			
			System.out.println(it.next().getX());
		
		}
		
		//Steram ��Ű�� : �÷��ǿ��� ���͸� ���ļ� � �������� ������ ���ҋ��� ����
		//				�Ʒ��� ���� ����Ѵ�.
		//����, �ߺ�, ������,n����� ���� ������ ������ �ɼ��ִ� (�߰�����)
		//�� �����͸� ��Ű¡�ϰų� �ٸ����·� �ٲٴ°�, �÷�Ʈ, ����ġ, ī��Ʈ �� (��������)
		
		int sum = stList.stream()
				.filter(st -> st.getX() == 1)
				.mapToInt(st-> st.getY())
				.sum();
		
		System.out.println(sum);
	
		
	
	
	}

	
	
}
