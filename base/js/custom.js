$(document).ready(function(){
	
	$('body,html').stop().animate({'scrollTop':0},1500,'swing');
	
	$(window).on('scroll',function(){
		var scroll=$(this).scrollTop();
		$('h1').text(scroll);
	
		/*//기존의 시작값을 기준으로 스크롤값이 적용되어야 하므로 각각의 시작값에 스크롤값을 더한다
		$('section>article').eq(0).css({'transform':'translateZ('+(0+scroll)+'px)'});
		$('section>article').eq(1).css({"transform":"translateZ(" +(-5000+scroll)+ "px)"});
		$('section>article').eq(2).css({"transform":"translateZ(" +(-10000+scroll)+ "px)"});
		$('section>article').eq(3).css({"transform":"translateZ(" +(-15000+scroll)+ "px)"});
		$('section>article').eq(4).css({"transform":"translateZ(" +(-20000+scroll)+ "px)"});*/
		
		
		//위 패턴이 단순하고 반복적이어서 공통된 규칙이 발견된다. 이를 반복문으로 재구성해 적용한다(동일한 작동이 연출된다)
		for(var i=0; i<5; i++){
			$('section>article').eq(i).css({'transform':'translateZ(' +(-5000*i+scroll)+ 'px)'});
		}
		
		/*//z축 위치에 따른 메뉴 활성화하기
		if(scroll>=0 && scroll<5000){
			$('.scrollNavi li').removeClass();
			$('.scrollNavi li').eq(0).addClass('on');
			$('article').removeClass();
			$('article').eq(0).addClass('on');	
		}
		if(scroll>=5000 && scroll<10000){
			$('.scrollNavi li').removeClass();
			$('.scrollNavi li').eq(1).addClass('on');
			$('article').removeClass();
			$('article').eq(1).addClass('on');	
		}
		if(scroll>=10000 && scroll<15000){
			$('.scrollNavi li').removeClass();
			$('.scrollNavi li').eq(2).addClass('on');
			$('article').removeClass();
			$('article').eq(2).addClass('on');	
		}
		if(scroll>=15000 && scroll<20000){
			$('.scrollNavi li').removeClass();
			$('.scrollNavi li').eq(3).addClass('on');
			$('article').removeClass();
			$('article').eq(3).addClass('on');	
		}
		if(scroll>=20000 && scroll<25000){
			$('.scrollNavi li').removeClass();
			$('.scrollNavi li').eq(4).addClass('on');
			$('article').removeClass();
			$('article').eq(4).addClass('on');	
		}*/
		
		//위 패턴도 동일한 패턴을 반복하고 있음, 이를 다시 반복문에 적용하기 위해, 위에서 작성한 반복문 속에 if문으로 구성함
		for(var i=0; i<5; i++){
			$('section>article').eq(i).css({'transform':'translateZ(' +(-5000*i+scroll)+ 'px)'});
			
			// 여기서 버튼의 변환이 보다 빨리 이뤄지게 하고싶다면 if(scroll>=i*5000-2500 && scroll< (i+1)*5000-2500)와 같이 적용해도 된다
			if(scroll>=i*5000 && scroll< (i+1)*5000){
				$('.scrollNavi li').removeClass();
				$('.scrollNavi li').eq(i).addClass('on');
				$('article').removeClass();
				$('article').addClass('on');	
			}
			
		}
	
		
	});
	
	
		/*
		//메뉴 클릭하면 해당 아티클로 이동하기
		$('.scrollNavi li').eq(0).on('click',function(){
			$('html,body').stop().animate({'scrollTop':0});
		});
		$('.scrollNavi li').eq(1).on('click',function(){
			$('html,body').stop().animate({'scrollTop':5000});
		});
		$('.scrollNavi li').eq(2).on('click',function(){
			$('html,body').stop().animate({'scrollTop':10000});
		});
		$('.scrollNavi li').eq(3).on('click',function(){
			$('html,body').stop().animate({'scrollTop':15000});
		});
		$('.scrollNavi li').eq(4).on('click',function(){
			$('html,body').stop().animate({'scrollTop':20000});
		});
		*/
	
		//위에것도 반복적이고 동일한 패턴이 보인다. index()구문을 이용해 정리한다
		$('.scrollNavi li').on('click',function(){
			var a = $(this).index();
			$('html,body').stop().animate({'scrollTop':5000*a},1500,'swing');
		});
		
		
	
});















