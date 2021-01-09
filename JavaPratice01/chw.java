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
		
		//Steram 패키지 : 컬렉션에서 필터를 거쳐서 어떤 정보들의 총합을 구할떄와 같이
		//				아래와 같이 사용한다.
		//필터, 중복, 사이즈,n개요소 제외 등으로 조건을 걸수있다 (중간연산)
		//위 데이터를 패키징하거나 다른형태로 바꾸는것, 컬렉트, 포이치, 카운트 등 (최종연산)
		
		int sum = stList.stream()
				.filter(st -> st.getX() == 1)
				.mapToInt(st-> st.getY())
				.sum();
		
		System.out.println(sum);
	
		
	
	
	}

	
	
}
