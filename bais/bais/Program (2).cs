using System;
using System.Collections.Generic;

namespace StarCraftOOP
{
    // 1. 공통 속성 열거형 (지상/공중, 생체/기계)
    public enum MoveType { Ground, Air }
    public enum UnitAttribute { Biological, Mechanical }

    // 2. 특수 기능 인터페이스 정의
    public interface ICloakable // 은신
    {
        bool IsCloaked { get; }
        void Cloak();
        void Decloak();
    }

    public interface IDetector // 탐지
    {
        void Detect();
    }

    public interface ITransport // 수송
    {
        void Load(Unit unit);
        void UnloadAll();
    }

    public interface IWorker // 자원 채취
    {
        void GatherResource();
    }

    public interface IBurrowable // 잠복
    {
        bool IsBurrowed { get; }
        void Burrow();
        void Unburrow();
    }

    // 3. 최상위 추상 클래스: Unit
    public abstract class Unit
    {
        public string Name { get; protected set; }
        public int Hp { get; protected set; }
        public int MaxHp { get; protected set; }
        public MoveType Movement { get; protected set; }
        public UnitAttribute Attribute { get; protected set; }

        protected Unit(string name, int maxHp, MoveType movement, UnitAttribute attribute)
        {
            Name = name;
            MaxHp = maxHp;
            Hp = maxHp;
            Movement = movement;
            Attribute = attribute;
        }

        // 공통 기능: 이동 (자식 클래스에서 재정의 가능하도록 virtual 선언)
        public virtual void Move(string destination)
        {
            string moveMode = Movement == MoveType.Air ? "비행하여" : "걸어서";
            Console.WriteLine($"[{Name}] ({Attribute}) {destination}(으)로 {moveMode} 이동합니다.");
        }

        // 피격 테스트용 메서드
        public virtual void TakeDamage(int damage)
        {
            Hp = Math.Max(0, Hp - damage);
            Console.WriteLine($"[{Name}] {damage}의 피해를 입었습니다. (남은 체력: {Hp}/{MaxHp})");
        }

        public abstract void ShowStatus();
    }

    // 4. 종족별 추상 클래스 (종족 고유 특성 구현)
    // 테란: 고유 패시브 없음
    public abstract class TerranUnit : Unit
    {
        protected TerranUnit(string name, int maxHp, MoveType movement, UnitAttribute attribute)
            : base(name, maxHp, movement, attribute) { }

        public override void ShowStatus()
        {
            Console.WriteLine($"[테란] {Name} | 체력: {Hp}/{MaxHp} | 타입: {Movement}, {Attribute}");
        }
    }

    // 저그: 체력 재생 특성
    public abstract class ZergUnit : Unit
    {
        protected ZergUnit(string name, int maxHp, MoveType movement, UnitAttribute attribute)
            : base(name, maxHp, movement, attribute) { }

        public void RegenerateHp(int amount)
        {
            if (Hp < MaxHp)
            {
                Hp = Math.Min(MaxHp, Hp + amount);
                Console.WriteLine($"[저그 특성 - 체력 재생] {Name}의 체력이 {amount} 회복되었습니다. ({Hp}/{MaxHp})");
            }
        }

        public override void ShowStatus()
        {
            Console.WriteLine($"[저그] {Name} | 체력: {Hp}/{MaxHp} | 타입: {Movement}, {Attribute}");
        }
    }

    // 프로토스: 방어막 특성
    public abstract class ProtossUnit : Unit
    {
        public int Shield { get; protected set; }
        public int MaxShield { get; protected set; }

        protected ProtossUnit(string name, int maxHp, int maxShield, MoveType movement, UnitAttribute attribute)
            : base(name, maxHp, movement, attribute)
        {
            MaxShield = maxShield;
            Shield = maxShield;
        }

        public override void TakeDamage(int damage)
        {
            if (Shield >= damage)
            {
                Shield -= damage;
                Console.WriteLine($"[{Name}] 방어막으로 {damage}의 피해를 흡수했습니다. (남은 방어막: {Shield}/{MaxShield})");
            }
            else
            {
                int remainingDamage = damage - Shield;
                Shield = 0;
                Hp = Math.Max(0, Hp - remainingDamage);
                Console.WriteLine($"[{Name}] 방어막이 파괴되고 체력에 {remainingDamage}의 피해를 입었습니다. (방어막: 0/{MaxShield}, 체력: {Hp}/{MaxHp})");
            }
        }

        public override void ShowStatus()
        {
            Console.WriteLine($"[프로토스] {Name} | 체력: {Hp}/{MaxHp}, 방어막: {Shield}/{MaxShield} | 타입: {Movement}, {Attribute}");
        }
    }

    // 5. 구체적인 유닛 클래스 구현 (인터페이스 조합)
    // 테란 - 건설로봇 (지상, 기계, 일꾼)
    public class SCV : TerranUnit, IWorker
    {
        public SCV() : base("건설로봇(SCV)", 45, MoveType.Ground, UnitAttribute.Mechanical) { }

        public void GatherResource()
        {
            Console.WriteLine($"[{Name}] 광물과 가스를 채취합니다.");
        }
    }

    // 테란 - 레이스 (공중, 기계, 은신)
    public class Wraith : TerranUnit, ICloakable
    {
        public bool IsCloaked { get; private set; }
        public Wraith() : base("망령(Wraith)", 120, MoveType.Air, UnitAttribute.Mechanical) { }

        public void Cloak()
        {
            IsCloaked = true;
            Console.WriteLine($"[{Name}] 은신 장치를 가동합니다. (적에게 보이지 않음)");
        }

        public void Decloak()
        {
            IsCloaked = false;
            Console.WriteLine($"[{Name}] 은신을 해제합니다.");
        }
    }

    // 테란 - 과학선 (공중, 기계, 탐지)
    public class ScienceVessel : TerranUnit, IDetector
    {
        public ScienceVessel() : base("과학선(Science Vessel)", 200, MoveType.Air, UnitAttribute.Mechanical) { }

        public void Detect()
        {
            Console.WriteLine($"[{Name}] 탐지기로 주변의 은신/잠복 유닛을 스캔합니다.");
        }
    }

    // 저그 - 저글링 (지상, 생체, 잠복)
    public class Zergling : ZergUnit, IBurrowable
    {
        public bool IsBurrowed { get; private set; }
        public Zergling() : base("저글링(Zergling)", 35, MoveType.Ground, UnitAttribute.Biological) { }

        public void Burrow()
        {
            IsBurrowed = true;
            Console.WriteLine($"[{Name}] 땅속으로 잠복했습니다.");
        }

        public void Unburrow()
        {
            IsBurrowed = false;
            Console.WriteLine($"[{Name}] 잠복을 해제하고 지상으로 올라옵니다.");
        }

        public override void Move(string destination)
        {
            if (IsBurrowed)
            {
                Console.WriteLine($"[{Name}] 잠복 상태에서는 이동할 수 없습니다.");
                return;
            }
            base.Move(destination);
        }
    }

    // 저그 - 대군주 (공중, 생체, 수송 + 탐지)
    public class Overlord : ZergUnit, ITransport, IDetector
    {
        private List<Unit> cargo = new List<Unit>();
        public Overlord() : base("대군주(Overlord)", 200, MoveType.Air, UnitAttribute.Biological) { }

        public void Load(Unit unit)
        {
            if (unit.Movement == MoveType.Air)
            {
                Console.WriteLine($"[{Name}] 공중 유닛({unit.Name})은 수송할 수 없습니다.");
                return;
            }
            cargo.Add(unit);
            Console.WriteLine($"[{Name}] {unit.Name}을(를) 수송선에 태웠습니다.");
        }

        public void UnloadAll()
        {
            Console.WriteLine($"[{Name}] 수송 중인 모든 유닛({cargo.Count}기)을 내립니다.");
            cargo.Clear();
        }

        public void Detect()
        {
            Console.WriteLine($"[{Name}] 예리한 감각으로 주변의 은신 유닛을 감지합니다.");
        }
    }

    // 프로토스 - 암흑 기사 (지상, 생체, 영구 은신)
    public class DarkTemplar : ProtossUnit, ICloakable
    {
        public bool IsCloaked { get; private set; } = true;
        public DarkTemplar() : base("암흑 기사(Dark Templar)", 40, 80, MoveType.Ground, UnitAttribute.Biological) { }

        public void Cloak() => Console.WriteLine($"[{Name}] 이미 영구 은신 상태입니다.");
        public void Decloak() => Console.WriteLine($"[{Name}] 은신을 해제할 수 없습니다.");
    }

    // 프로토스 - 탐사정 (지상, 기계, 일꾼)
    public class Probe : ProtossUnit, IWorker
    {
        public Probe() : base("탐사정(Probe)", 20, 20, MoveType.Ground, UnitAttribute.Mechanical) { }

        public void GatherResource()
        {
            Console.WriteLine($"[{Name}] 입자 광선으로 자원을 채취합니다.");
        }
    }

    // 6. 메인 실행부 (다형성 테스트)
    class Program
    {
        static void Main(string[] args)
        {
            // 다양한 종족과 타입의 유닛을 부모 클래스(Unit) 리스트로 통합 관리
            List<Unit> units = new List<Unit>
            {
                new SCV(),
                new Wraith(),
                new ScienceVessel(),
                new Zergling(),
                new Overlord(),
                new DarkTemplar(),
                new Probe()
            };

            Console.WriteLine("=== 1. 전체 유닛 상태 및 이동 (추상 클래스 다형성) ===");
            foreach (var unit in units)
            {
                unit.ShowStatus();
                unit.Move("중앙 격전지");
            }

            Console.WriteLine("\n=== 2. 종족별 고유 특성 테스트 ===");
            foreach (var unit in units)
            {
                unit.TakeDamage(30); // 모두 30 데미지 피격

                // 저그 유닛은 체력 재생
                if (unit is ZergUnit zerg)
                {
                    zerg.RegenerateHp(10);
                }
            }

            Console.WriteLine("\n=== 3. 인터페이스 다형성 활용 (특수 능력 일괄 실행) ===");
            foreach (var unit in units)
            {
                // 일꾼 기능
                if (unit is IWorker worker)
                {
                    worker.GatherResource();
                }

                // 은신 기능
                if (unit is ICloakable cloakable)
                {
                    cloakable.Cloak();
                }

                // 탐지 기능
                if (unit is IDetector detector)
                {
                    detector.Detect();
                }

                // 잠복 기능
                if (unit is IBurrowable burrowable)
                {
                    burrowable.Burrow();
                    unit.Move("적진"); // 잠복 후 이동 불가 확인
                }
            }

            Console.WriteLine("\n=== 4. 수송(ITransport) 기능 테스트 ===");
            ITransport transport = new Overlord();
            transport.Load(units[0]); // SCV(지상) 탑승
            transport.Load(units[1]); // Wraith(공중) 탑승 시도 -> 실패
            transport.UnloadAll();
        }
    }
}
