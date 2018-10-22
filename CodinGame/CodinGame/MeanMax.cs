using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Math = System.Math;

namespace Contest.MeanMax
{
    #region ConsoleHelper
    public interface IConsoleHelper
    {
        string ReadLine();
        T ReadLineAs<T>();

        string[] ReadLineAndSplit(char delimiter = ' ');
        List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ');

        void WriteLine(object obj);
        void Debug(object obj);
    }

    public class ConsoleHelper : IConsoleHelper
    {
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }

        public T ReadLineAs<T>()
        {
            var line = this.ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = this.ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = this.ReadLineAndSplit();

            return splittedLine.Select(ConvertTo<T>).ToList();
        }

        public virtual void WriteLine(object obj)
        {
            Console.WriteLine(obj);
        }

        public void Debug(object obj)
        {
            Console.Error.WriteLine(obj);
        }

        private static T ConvertTo<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
    #endregion

    #region Builder
    public interface IEntityBuilder
    {
        Entity Build(string[] inputs);
    }

    public class EntityBuilder : IEntityBuilder
    {
        public Entity Build(string[] inputs)
        {
            Entity entity;

            var id = int.Parse(inputs[0]);
            var type = int.Parse(inputs[1]);
            var player = int.Parse(inputs[2]);

            var mass = float.Parse(inputs[3]);
            var radius = int.Parse(inputs[4]);

            var pos = new Position
            {
                X = int.Parse(inputs[5]),
                Y = int.Parse(inputs[6])
            };
            var speed = new Speed
            {
                Vx = int.Parse(inputs[7]),
                Vy = int.Parse(inputs[8])
            };

            var remaining = int.Parse(inputs[9]);
            var maxWater = int.Parse(inputs[10]);

            switch (type)
            {
                case 0:
                    var reaper = new Reaper
                    {
                        Player = player,
                        Speed = speed
                    };
                    entity = reaper;
                    break;
                case 1:
                    var destroyer = new Destroyer
                    {
                        Player = player,
                        Speed = speed
                    };
                    entity = destroyer;
                    break;
                case 2:
                    var doof = new Doof
                    {
                        Player = player,
                        Speed = speed
                    };
                    entity = doof;
                    break;
                case 3:
                    var tanker = new Tanker
                    {
                        Speed = speed,
                        AvailableWater = remaining,
                        MaxWater = maxWater
                    };
                    entity = tanker;
                    break;
                case 4:
                    var wreck = new Wreck
                    {
                        AvailableWater = remaining
                    };
                    entity = wreck;
                    break;
                case 5:
                    var tar = new Tar
                    {
                        RemainingRound = remaining
                    };
                    entity = tar;
                    break;
                case 6:
                    var oil = new Oil
                    {
                        RemainingRound = remaining
                    };
                    entity = oil;
                    break;
                default:
                    throw new NotImplementedException($"type '{type}' is not supported.");
            }

            entity.Id = id;
            entity.Pos = pos;
            entity.Radius = radius;

            return entity;
        }
    }
    #endregion

    #region Model
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {
            
        }
        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool IsInRange2(Position pos, int range)
        {
            var dist2 = this.Dist2(pos);
            return dist2 <= range;
        }

        public int Dist2(Position pos)
        {
            var dx = this.X - pos.X;
            var dy = this.Y - pos.Y;
            return dx * dx + dy * dy;
        }

        public double Dist(Position pos)
        {
            return Math.Sqrt(this.Dist2(pos));
        }

        public Position Center(Position pos)
        {
            var x = (this.X - pos.X) / 2;
            var y = (this.Y - pos.Y) / 2;
            return new Position(x, y);
        }

        public Position Clone()
        {
            return new Position(this.X, this.Y);
        }

        public override string ToString()
        {
            return $"X={this.X,-5} Y={this.Y,-5}";
        }
    }

    public class Speed
    {
        public int Vx { get; set; }
        public int Vy { get; set; }

        public double GetNorme()
        {
            return Math.Sqrt(this.Vx * this.Vx + this.Vy * this.Vy);
        }

        public override string ToString()
        {
            return $"Vx={this.Vx,-5} Vy={this.Vy,-5}";
        }
    }

    public abstract class Entity
    {
        public int Id { get; set; }
        public Position Pos { get; set; }
        public int Radius { get; set; }

        public virtual Position EstimateNextPosition()
        {
            return this.Pos;
        }

        public int Dist2(Entity entity)
        {
            return this.Pos.Dist2(entity.Pos);
        }

        public double Dist(Entity entity)
        {
            return this.Pos.Dist(entity.Pos);
        }

        public Position Center(Entity entity)
        {
            return this.Pos.Center(entity.Pos);
        }

        public int CountNearEntity(int range2, params Entity[] entities)
        {
            return entities.Count(e => this.Dist2(e) <= range2);
        }

        public Entity GetClosest(IEnumerable<Entity> entities)
        {
            var minDist2 = int.MaxValue;
            Entity closest = null;
            foreach (var entity in entities)
            {
                var dist2 = this.Dist2(entity);
                if (dist2 < minDist2)
                {
                    minDist2 = dist2;
                    closest = entity;
                }
            }
            return closest;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }

        public override string ToString()
        {
            return $"[{this.Id,-2}] {this.Pos} Radius={this.Radius,-4}";
        }
    }

    public abstract class Unit : Entity
    {
        public abstract float Mass { get; }
        public Speed Speed { get; set; }
        public abstract float Friction { get; }
        public abstract int Acc { get; }

        public override Position EstimateNextPosition()
        {
            var acc = this.Acc / this.Mass;
            var nextVx = (int)Math.Round((this.Speed.Vx + acc) * (1 - this.Friction));
            var nextVy = (int)Math.Round((this.Speed.Vy + acc) * (1 - this.Friction));

            var x = this.Pos.X + nextVx;
            var y = this.Pos.Y + nextVy;
            return new Position
            {
                X = x,
                Y = y
            };
        }

        public override string ToString()
        {
            return $"{base.ToString()} Mass={this.Mass,-5} {this.Speed} Acc={this.Acc,-5}";
        }
    }

    public abstract class Looter : Unit
    {
        public int Player { get; set; }
        public override int Acc { get; } = 0;

        public Move ComputeMove(Position dest)
        {
            var move = new Move
            {
                Pos = new Position
                {
                    X = dest.X - this.Speed.Vx,
                    Y = dest.Y - this.Speed.Vy
                }
            };

            // Acc
            var dist = this.Pos.Dist(move.Pos);
            if (Math.Abs(dist) <= 0.0001)
                return move;

            //var expectedVx = Math.Abs(move.Pos.X - this.Pos.X) / (1 - this.Friction);
            //var expectedVy = Math.Abs(move.Pos.Y - this.Pos.Y) / (1 - this.Friction);

            //var accX = (int)Math.Round(expectedVx * dist * this.Mass);
            //var accY = (int)Math.Round(expectedVy * dist * this.Mass);

            //Program.ConsoleHelper.Debug($"accX={accX}|accY={accY}");
            //move.Acc = Math.Min((accX + accY) / 2, 300);

            var acc = (int)Math.Round(this.Mass * dist);
            move.Acc = Math.Min(acc, 300);
            return move;
        }

        public Position SimulateNextPosition(Position position)
        {
            var move = this.ComputeMove(position);
            if (move.Acc < 300)
                return position;

            var dist = this.Pos.Dist(move.Pos);
            var coef = move.Acc / this.Mass / dist;
            var realX = this.Pos.X + this.Speed.Vx + (position.X - this.Pos.X) * coef;
            var realY = this.Pos.Y + this.Speed.Vy + (position.Y - this.Pos.Y) * coef;
            return new Position((int)realX, (int)realY);
        }

        public override string ToString()
        {
            return $"{base.ToString()} Player={this.Player}";
        }
    }

    public class Reaper : Looter
    {
        public override float Mass => 0.5f;
        public override float Friction => 0.2f;

        public override string ToString()
        {
            return $"{"Reaper",-9} {base.ToString()}";
        }
    }

    public class Destroyer : Looter
    {
        public override float Mass => 1.5f;
        public override float Friction => 0.3f;

        public override string ToString()
        {
            return $"{"Destroyer",-9} {base.ToString()}";
        }
    }

    public class Doof : Looter
    {
        public override float Mass => 1f;
        public override float Friction => 0.25f;

        public int ComputeRage()
        {
            var rage = (int)Math.Floor(this.Speed.GetNorme() / 100);
            return Math.Min(rage, 300);
        }

        public override string ToString()
        {
            return $"{"Doof",-9} {base.ToString()}";
        }
    }

    public class Tanker : Unit
    {
        public override float Mass => 2.5f + 0.5f * this.AvailableWater;
        public override float Friction => 0.4f;
        public override int Acc => 500;

        public int AvailableWater { get; set; }
        public int MaxWater { get; set; }

        public override string ToString()
        {
            return $"{"Tanker",-9} {base.ToString()} Water={this.AvailableWater} MaxWater={this.MaxWater}";
        }
    }

    public class Wreck : Entity
    {
        public int WreckId { get; set; }

        public int AvailableWater { get; set; }

        public override string ToString()
        {
            return $"{"Wreck",-9} {base.ToString()} Water={this.AvailableWater}";
        }

        public string ToStringIdOnly()
        {
            return $"{this.Id}({this.WreckId})";
        }
    }

    public abstract class Skill : Entity
    {
        public int RemainingRound { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()} RemainingRound={this.RemainingRound}";
        }
    }

    public class Tar : Skill
    {
        public override string ToString()
        {
            return $"{"Tar",-9} {base.ToString()}";
        }
    }

    public class Oil : Skill
    {
        public override string ToString()
        {
            return $"{"Oil",-9} {base.ToString()}";
        }
    }

    public class Player
    {
        public Reaper Reaper { get; set; }
        public Destroyer Destroyer { get; set; }
        public Doof Doof { get; set; }

        public int Score { get; set; }
        public int Rage { get; set; }
    }

    public class Game
    {
        public Player Me => Players[0];
        public Player[] Players { get; set; }

        public List<Tanker> Tankers { get; set; } = new List<Tanker>();
        public List<Wreck> Wrecks { get; set; } = new List<Wreck>();
        public List<Tar> Tars { get; set; } = new List<Tar>();
        public List<Oil> Oils { get; set; } = new List<Oil>();
    }
    #endregion

    #region Model for Simu

    public class WreckGroup
    {
        public Position Center { get; set; }
        public int Range2 { get; set; }

        public HashSet<Wreck> Wrecks { get; set; }
        public int Weight { get; set; }

        public Position BestPos { get; set; }
        public int BestPosWeight { get; set; }
        public bool IsBestPosInter { get; set; }

        public WreckGroup(Wreck wreck)
        {
            this.Center = wreck.Pos.Clone();
            this.Range2 = 0;

            this.Wrecks = new HashSet<Wreck> {wreck};
            this.Weight = wreck.AvailableWater;
        }

        public WreckGroup(Wreck wreck1, Wreck wreck2, int range2)
        {
            this.Center = wreck1.Center(wreck2);
            this.Range2 = range2;
            this.Wrecks = new HashSet<Wreck> {wreck1, wreck2};
            this.Weight = wreck1.AvailableWater + wreck2.AvailableWater;
        }

        public void Add(Wreck wreck, int range2, Wreck farestWreck)
        {
            if (range2 > this.Range2)
            {
                this.Range2 = range2;
                this.Center = wreck.Center(farestWreck);
            }

            this.Wrecks.Add(wreck);
            this.Weight += wreck.AvailableWater;
        }

        public Position GetLimit(Entity entity)
        {
            if (this.IsBestPosInter)
                return this.BestPos;

            var wreck = this.Wrecks.First();
            var radius2 = (wreck.Radius * wreck.Radius * 9) / 10;
            var dist2 = entity.EstimateNextPosition().Dist2(wreck.Pos);
            //new ConsoleHelper() .Debug($"GETLIMIT = Radius={wreck.Radius} Dist={Math.Sqrt(dist2)}");
            if (dist2 < radius2)
                return this.BestPos;

            var coef = radius2 * 1d / dist2;
            var x = this.BestPos.X * (1 - coef) + entity.Pos.X * coef;
            var y = this.BestPos.Y * (1 - coef) + entity.Pos.Y * coef;
            return new Position((int)x, (int)y);
        }

        public override string ToString()
        {
            var groupInfo = $"{this.Center} Range={Math.Sqrt(this.Range2)} Weight={this.Weight} WreckCount={this.Wrecks.Count}";
            var bestPosInfo = $"BestPos={this.BestPos} BestWeight={this.BestPosWeight} IsInter={this.IsBestPosInter}";
            return $"WreckGroup {groupInfo} || {bestPosInfo}";
        }
    }

    public class Segment
    {
        public Position Start { get; set; }
        public Position End { get; set; }
        public int Radius { get; set; }

        private readonly Droite droite;

        public Segment(Position start, Position end, int radius)
        {
            this.Start = start;
            this.End = end;
            this.Radius = radius;

            this.droite = DroiteFactory.Build(start, end);
            //new ConsoleHelper().Debug($"DROITE for start={start} end={end} {droite}");
        }

        public bool IsBlocker(Entity entity)
        {
            var h = this.GetProjeteOrthogonal(entity.Pos);
            //new ConsoleHelper().Debug($"PROJETE for {entity.Id} = {h}");
            var dist2 = h.Dist2(entity.Pos);
            var r = this.Radius + entity.Radius;

            var isBlocker = dist2 <= r * r;
            //new ConsoleHelper().Debug($"isBlocker={isBlocker} dist={Math.Sqrt(dist2)} radiusTotal={r} RadiusReaper={this.Radius} RadiusEntity={entity.Radius}");
            return isBlocker && this.Contains(entity);
        }

        public Position GetProjeteOrthogonal(Position position)
        {
            return this.droite.GetProjeteOrthogonal(position);
        }

        public bool Contains(Entity entity)
        {
            var r = this.Radius + entity.Radius;
            var x = this.Start.X < this.End.X
                ? this.Start.X - r <= entity.Pos.X && entity.Pos.X <= this.End.X + r
                : this.End.X - r <= entity.Pos.X && entity.Pos.X <= this.Start.X + r;
            var y = this.Start.Y < this.End.Y
                ? this.Start.Y - r <= entity.Pos.Y && entity.Pos.Y <= this.End.Y + r
                : this.End.Y - r <= entity.Pos.Y && entity.Pos.Y <= this.Start.Y + r;
            return x && y;
        }
    }

    public class DroiteFactory
    {
        public static Droite Build(Position start, Position end)
        {
            var dx = end.X - start.X;
            var dy = end.Y - start.Y;

            if (dx == 0)
                return new DroiteVertical {A = end.X};

            if (dy == 0)
                return new DroiteHorizontal {B = end.Y};

            var a = dy;
            var b = -dx;
            var c = -a * end.X - b * end.Y;
            return new Droite {A = a, B = b, C = c};
        }
    }

    public class Droite
    {
        // ax + by + c = 0
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public virtual Position GetProjeteOrthogonal(Position position)
        {
            var a = -this.B;
            var b = this.A;
            var c = -a * position.X - b * position.Y;
            //new ConsoleHelper().Debug($"PERP a={a} b={b} c={c}");

            var x = (this.B * c - b * this.C) / (b * this.A - this.B * a);
            var y = (this.A * c - a * this.C) / (a * this.B - this.A * b);
            return new Position((int)x, (int)y);
        }

        public override string ToString()
        {
            return $"a={this.A} b={this.B} c={this.C}";
        }
    }

    public class DroiteHorizontal : Droite
    {
        // y = b
        public override Position GetProjeteOrthogonal(Position position)
        {
            return new Position(position.X, (int)this.B);
        }
    }

    public class DroiteVertical : Droite
    {
        // x = a
        public override Position GetProjeteOrthogonal(Position position)
        {
            return new Position((int)this.A, position.X);
        }
    }
    #endregion

    #region Movement

    public class RoundDecision
    {
        public IAction ReaperAction { get; set; }
        public IAction DestroyerAction { get; set; }
        public IAction DoofAction { get; set; }
    }
    public interface IAction
    {
        string Execute();
    }

    public abstract class Action : IAction
    {
        public abstract string Execute();

        public override string ToString()
        {
            return this.Execute();
        }
    }

    public class Move : Action
    {
        public Position Pos { get; set; }
        public int Acc { get; set; }

        public override string Execute()
        {
            return $"{this.Pos.X} {this.Pos.Y} {this.Acc}";
        }
    }

    public class UseSkill : Action
    {
        public Position Target { get; set; }

        public override string Execute()
        {
            return $"SKILL {this.Target.X} {this.Target.Y}";
        }
    }

    public class Wait : Action
    {
        public override string Execute()
        {
            return "WAIT";
        }
    }
    #endregion

    public static class Helper
    {
        public static IConsoleHelper ConsoleHelper = new ConsoleHelper();

        public static void Debug<T>(this T item)
        {
            if (item is IEnumerable enumerable)
            {
                enumerable.Debug();
            }
            else
            {
                ConsoleHelper.Debug(item);
            }
        }

        public static void Debug(this IEnumerable items)
        {
            foreach (var item in items)
                item.Debug();
        }
    }

    public static class Program
    {
        public static IConsoleHelper ConsoleHelper = new ConsoleHelper();
        public static IEntityBuilder EntityBuilder = new EntityBuilder();

        public const int Trace = 0;

        public const int Radius = 6000;
        public const int Radius2 = 18000000;
        public static Position Center = new Position {X = 0, Y = 0};

        public const int SkillRange = 2000;
        public const int SkillRange2 = SkillRange * SkillRange;

        public static void Main(string[] args)
        {
            Solve();
        }

        public static void Solve()
        {
            while (true)
            {
                var sw = new Stopwatch();
                sw.Start();
                var game = ReadRound();

                var roundDecision = Heuristic(game);
                //var roundDecision = AG(game);

                ConsoleHelper.WriteLine(roundDecision.ReaperAction.Execute());
                ConsoleHelper.WriteLine(roundDecision.DestroyerAction.Execute());
                ConsoleHelper.WriteLine(roundDecision.DoofAction.Execute());

                sw.Stop();
                ConsoleHelper.Debug($"Time={sw.Elapsed.TotalMilliseconds}");
            }
        }

        //private static RoundDecision AG(Game game)
        //{
           
        //    var roundDecision = new RoundDecision()
        //    {
        //        ReaperAction = reaperAction,
        //        DestroyerAction = destroyerAction,
        //        DoofAction = doofAction,
        //    };
        //    return roundDecision;
        //}

        private static RoundDecision Heuristic(Game game)
        {
            var groups = BuildGroups(game.Wrecks, game.Me.Reaper.Pos);
            ConsoleHelper.Debug(groups.Count);
            groups.Debug();

            var ennemies = game.Players.Skip(1).OrderByDescending(p => p.Score).ToArray();
            var bestEnnemyPlayer = ennemies[0];
            var worstEnnemyPlayer = ennemies[1];

            // Destroyer
            var closestTanker = game.Tankers
                .Select(tanker => new {Tanker = tanker, NextPos = tanker.EstimateNextPosition()})
                .Where(tanker => tanker.NextPos.IsInRange2(Center, (int) (Radius2 * 0.9)))
                .OrderBy(tanker => tanker.NextPos.Dist2(game.Me.Destroyer.Pos))
                .ThenByDescending(tanker => tanker.Tanker.AvailableWater)
                .FirstOrDefault();
            IAction destroyerAction = new Wait();
            var ennemyCloseToReaper = game.Me.Reaper.CountNearEntity(SkillRange2, game.Players[1].Destroyer,
                game.Players[1].Doof, game.Players[2].Destroyer, game.Players[2].Doof);
            if (game.Me.Rage >= 150
                && game.Me.Reaper.Pos.IsInRange2(game.Me.Destroyer.Pos, SkillRange2)
                && ennemyCloseToReaper > 1)
            {
                destroyerAction = new UseSkill {Target = game.Me.Reaper.Pos};
            }
            else if (closestTanker != null)
            {
                var nextPos = closestTanker.NextPos;
                var move = game.Me.Destroyer.ComputeMove(nextPos);
                destroyerAction = move;
            }

            // Reaper
            var closestReaper = game.Wrecks
                .OrderBy(wreck => wreck.Dist2(game.Me.Reaper))
                .ThenByDescending(wreck => wreck.AvailableWater)
                .FirstOrDefault();
            var closestGroup = groups
                .OrderByDescending(g => g.BestPosWeight - g.BestPos.Dist2(game.Me.Reaper.Pos) / 1000)
                .FirstOrDefault();
            IAction reaperAction = destroyerAction;
            if (closestGroup != null)
            {
                //var nextPos = game.Me.Reaper.GetClosest(closestGroup.Wrecks).Pos;
                var nextPos = closestGroup.BestPos;
                // if i'm in => protect
                if (game.Me.Reaper.Pos.IsInRange2(nextPos, 1000000))
                {
                    var limitPos = closestGroup.GetLimit(bestEnnemyPlayer.Reaper);
                    ConsoleHelper.Debug($"PROTECT : {nextPos} to {limitPos} (for {bestEnnemyPlayer.Reaper.Id})");
                    nextPos = limitPos;
                }
                else 
                {
                    // if someone block me => go out
                    ConsoleHelper.Debug($"NEXT POS={nextPos}");
                    var units = new List<Unit>(game.Tankers)
                    {
                        game.Players[1].Destroyer,
                        game.Players[1].Doof,
                        game.Players[2].Destroyer,
                        game.Players[2].Doof
                    };
                    const int delta = 20;
                    var alphas = new Queue<double>();
                    alphas.Enqueue(0);
                    for (var alpha = delta; alpha < 360; alpha += delta)
                    {
                        alphas.Enqueue(alpha);
                        alphas.Enqueue(-alpha);
                    }
                    var ok = false;
                    while (!ok)
                    {
                        if (alphas.Count == 0)
                            break;

                        var alpha = alphas.Dequeue();
                        var cos = Math.Cos(alpha);
                        var sin = Math.Sin(alpha);
                        var dx = nextPos.X - game.Me.Reaper.Pos.X;
                        var dy = nextPos.Y - game.Me.Reaper.Pos.Y;
                        var x = game.Me.Reaper.Pos.X + dx * cos + dy * sin;
                        var y = game.Me.Reaper.Pos.Y + dy * cos + dx * sin;
                        var pos = new Position((int)x, (int)y);
                        ConsoleHelper.Debug($"CHECK alpha={alpha} pos={pos}");
                        var realNextPos = game.Me.Reaper.SimulateNextPosition(pos);
                        ConsoleHelper.Debug($"realpos={realNextPos}");
                        var segment = new Segment(game.Me.Reaper.Pos, realNextPos, game.Me.Reaper.Radius);

                        var blockers = units.Where(u => segment.IsBlocker(u)).ToList();
                        if (blockers.Count == 0)
                        {
                            ok = true;
                            nextPos = pos;
                        }
                        else
                        {
                            var ids = string.Join(";", blockers.Select(b => b.Id));
                            ConsoleHelper.Debug($"BLOCKER count={blockers.Count} {ids}");
                        }
                    }

                    var tankerOnBestPos = game.Tankers
                        .Where(t => closestGroup.BestPos.Dist2(t.Pos) <= t.Radius * t.Radius).ToList();
                    if (tankerOnBestPos.Count > 1)
                    {
                        ConsoleHelper.Debug("PROBLEM : Multiple Tankers on best pos");
                        // change dest
                    }
                    else if (tankerOnBestPos.Count == 1)
                    {
                        ConsoleHelper.Debug("PROBLEM : Tanker on best pos");
                        // adapt pos
                    }

                    // if oil problem
                    if (game.Oils.Count > 0 && game.Oils.Min(o => closestGroup.BestPos.Dist2(o.Pos)) <= SkillRange2)
                    {
                        ConsoleHelper.Debug("OIL PROBLEM");
                    }
                }
                var move = game.Me.Reaper.ComputeMove(nextPos);
                reaperAction = move;
            }
            else if (closestReaper != null)
            {
                var nextPos = closestReaper.Pos;
                var move = game.Me.Reaper.ComputeMove(nextPos);
                reaperAction = move;
            }

            // Doof
            // focus on 1st or 2nd ?
            var ennemyToFocus = game.Me.Score < worstEnnemyPlayer.Score ? worstEnnemyPlayer : bestEnnemyPlayer; 
            var doofMove = game.Me.Doof.ComputeMove(ennemyToFocus.Reaper.Pos);
            doofMove.Acc = 300;
            IAction doofAction = doofMove;
            if (game.Me.Rage >= 30
                && game.Me.Doof.Pos.IsInRange2(ennemyToFocus.Reaper.Pos, SkillRange2)
                && game.Wrecks.Any(w => w.Pos.IsInRange2(ennemyToFocus.Reaper.Pos, w.Radius * w.Radius)
                                        && !game.Me.Reaper.Pos.IsInRange2(ennemyToFocus.Reaper.Pos, SkillRange2)))
            {
                doofAction = new UseSkill {Target = ennemyToFocus.Reaper.Pos};
            }

            var roundDecision = new RoundDecision()
            {
                ReaperAction = reaperAction,
                DestroyerAction = destroyerAction,
                DoofAction = doofAction,
            };
            return roundDecision;
        }

        public static List<WreckGroup> BuildGroups(List<Wreck> wrecks, Position reaperPos)
        {
            var orderedWrecks = wrecks.OrderBy(w => w.Id).ToArray();
            for (var i = 0; i < orderedWrecks.Length; i++)
                orderedWrecks[i].WreckId = i;

            return BuildGroups(orderedWrecks, reaperPos);
        }

        public static List<WreckGroup> BuildGroups(Wreck[] wrecks, Position reaperPos)
        {
            const int MaxRange = 3000;
            const int MaxRange2 = MaxRange * MaxRange;

            // compute dist
            var dist2Matrix = new int[wrecks.Length, wrecks.Length];
            var sortetDist2 = new SortedList<int, Tuple<Wreck, Wreck>>();

            for (var i = 0; i < wrecks.Length; i++)
            for (var j = i + 1; j < wrecks.Length; j++)
            {
                var dist2 = wrecks[i].Dist2(wrecks[j]);
                dist2Matrix[i, j] = dist2Matrix[j, i] = dist2;
                sortetDist2.Add(dist2, new Tuple<Wreck, Wreck>(wrecks[i], wrecks[j]));
            }

            // compute group
            var groups = new List<WreckGroup>();
            var wreckGroups = new WreckGroup[wrecks.Length];
            foreach (var item in sortetDist2)
            {
                var wreck1 = item.Value.Item1;
                var wreck2 = item.Value.Item2;
                var wreckGroup1 = wreckGroups[wreck1.WreckId];
                var wreckGroup2 = wreckGroups[wreck2.WreckId];

                if (wreckGroup1 != null && wreckGroup2 != null)
                    continue;

                if (wreckGroup1 == null && wreckGroup2 == null)
                {
                    if (item.Key <= MaxRange2)
                    {
                        var group = new WreckGroup(wreck1, wreck2, item.Key);
                        wreckGroups[wreck1.WreckId] = group;
                        wreckGroups[wreck2.WreckId] = group;
                        groups.Add(group);
                    }
                    else
                    {
                        var group1 = new WreckGroup(wreck1);
                        wreckGroups[wreck1.WreckId] = group1;
                        groups.Add(group1);

                        var group2 = new WreckGroup(wreck2);
                        wreckGroups[wreck2.WreckId] = group2;
                        groups.Add(group2);
                    }
                }
                else
                {
                    WreckGroup group;
                    Wreck wreckToAdd;
                    if (wreckGroup1 == null)
                    {
                        group = wreckGroup2;
                        wreckToAdd = wreck1;
                    }
                    else
                    {
                        group = wreckGroup1;
                        wreckToAdd = wreck2;
                    }

                    var maxDist2ToGroup = int.MinValue;
                    Wreck farestWreck = null;
                    foreach (var wreck in group.Wrecks)
                    {
                        var dist2 = dist2Matrix[wreckToAdd.WreckId, wreck.WreckId];
                        if (dist2 >= maxDist2ToGroup)
                        {
                            maxDist2ToGroup = dist2;
                            farestWreck = wreck;
                        }
                    }
                    if (maxDist2ToGroup <= MaxRange2)
                    {
                        group.Add(wreckToAdd, maxDist2ToGroup, farestWreck);
                        wreckGroups[wreckToAdd.WreckId] = group;
                    }
                    else
                    {
                        var group2 = new WreckGroup(wreckToAdd);
                        wreckGroups[wreckToAdd.WreckId] = group2;
                        groups.Add(group2);
                    }
                }
            }

            // compute best pos for each group
            foreach (var @group in groups)
            {
                var groupWrecks = @group.Wrecks.OrderByDescending(w => w.AvailableWater).ThenBy(w => w.Pos.Dist2(reaperPos)).ToArray();
                @group.BestPos = groupWrecks[0].Pos;
                @group.BestPosWeight = groupWrecks[0].AvailableWater;
                @group.IsBestPosInter = false;
                for (var i = 0; i < groupWrecks.Length - 1; i++)
                for (var j = i + 1; j < groupWrecks.Length; j++)
                {
                    var wreck1 = groupWrecks[i];
                    var wreck2 = groupWrecks[j];
                    var weight = wreck1.AvailableWater + wreck2.AvailableWater;
                    if (weight <= @group.BestPosWeight)
                        continue;

                    var radius = wreck1.Radius + wreck2.Radius;
                    if (dist2Matrix[wreck1.WreckId, wreck2.WreckId] <= radius * radius)
                    {
                        var x = (wreck1.Pos.X * wreck2.Radius + wreck2.Pos.X * wreck1.Radius) / radius;
                        var y = (wreck1.Pos.Y * wreck2.Radius + wreck2.Pos.Y * wreck1.Radius) / radius;
                        @group.BestPos = new Position(x, y);
                        @group.BestPosWeight = weight;
                        @group.IsBestPosInter = true;
                    }
                }
            }

            return groups;
        }

        private static Game ReadRound()
        {
            var game = new Game
            {
                Players = new[]
                {
                    new Player(),
                    new Player(),
                    new Player()
                }
            };

            game.Players[0].Score = int.Parse(ConsoleHelper.ReadLine());
            game.Players[1].Score = int.Parse(ConsoleHelper.ReadLine());
            game.Players[2].Score = int.Parse(ConsoleHelper.ReadLine());

            game.Players[0].Rage = ConsoleHelper.ReadLineAs<int>();
            game.Players[1].Rage = ConsoleHelper.ReadLineAs<int>();
            game.Players[2].Rage = ConsoleHelper.ReadLineAs<int>();

            var unitCount = ConsoleHelper.ReadLineAs<int>();
            for (var i = 0; i < unitCount; i++)
            {
                var inputs = ConsoleHelper.ReadLineAndSplit();
                var entity = EntityBuilder.Build(inputs);
                if (Trace > 0)
                    ConsoleHelper.Debug(entity);

                // Not enabled on CG !
                //switch (entity)
                if (entity is Reaper reaper)
                {
                    game.Players[reaper.Player].Reaper = reaper;
                }
                else if (entity is Destroyer destroyer)
                {
                    game.Players[destroyer.Player].Destroyer = destroyer;
                }
                else if (entity is Doof doof)
                {
                    game.Players[doof.Player].Doof = doof;
                }
                else if (entity is Tanker tanker)
                {
                    game.Tankers.Add(tanker);
                }
                else if (entity is Wreck wreck)
                {
                    game.Wrecks.Add(wreck);
                }
                else if (entity is Tar tar)
                {
                    game.Tars.Add(tar);
                }
                else if (entity is Oil oil)
                {
                    game.Oils.Add(oil);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return game;
        }
    }
}
