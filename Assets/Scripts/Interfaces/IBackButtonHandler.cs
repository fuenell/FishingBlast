namespace FishingBlast.Interfaces
{
    public interface IBackButtonHandler
    {
        // 최상위 화면에서 뒤로가기 버튼이 눌렸을 때 호출될 계약
        void HandleBackButtonOnTop();
    }
}