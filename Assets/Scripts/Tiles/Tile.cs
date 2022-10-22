using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Tile : MonoBehaviour
{
    public string TileName;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _rangeHighlight;
    [SerializeField] public GameObject _heroHighlight;
    [SerializeField] public GameObject _enemyHighlight;
    [SerializeField] public int DmgBonus;
    [SerializeField] private bool _isWalkable;

    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;


    public virtual void Init(int x, int y)
    {

    }

    void OnMouseEnter()
    {
        var results = winCheck();
        if (results == 1)
        {
            GameManager.Instance.ChangeState(GameState.HeroesWin);
            return;
        }
        else if (results == 2)
        {
            GameManager.Instance.ChangeState(GameState.EnemiesWin);
            return;
        }

        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void OnMouseExit()
    {
        var results = winCheck();
        if (results == 1)
        {
            GameManager.Instance.ChangeState(GameState.HeroesWin);
            return;
        }
        else if (results == 2)
        {
            GameManager.Instance.ChangeState(GameState.EnemiesWin);
            return;
        }

        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    void OnMouseDown()
    {
        var results = winCheck();
        if (results == 1)
        {
            GameManager.Instance.ChangeState(GameState.HeroesWin);
            return;
        }
        else if (results == 2)
        {
            GameManager.Instance.ChangeState(GameState.EnemiesWin);
            return;
        }

        if (GameManager.Instance.GameState != GameState.HeroesTurn && GameManager.Instance.GameState != GameState.EnemiesTurn) return;

        if (GameManager.Instance.GameState == GameState.HeroesTurn)
        {

            if (OccupiedUnit != null)
            {
                if (OccupiedUnit.Faction == Faction.Hero && OccupiedUnit.turnTaken == false)
                {
                    UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
                    highlightRange(OccupiedUnit);
                }
                else
                {
                    if (UnitManager.Instance.SelectedHero != null)
                    {
                        if (isWithinRange(UnitManager.Instance.SelectedHero, this))
                        {
                            var enemy = (BaseEnemy)OccupiedUnit;
                            enemy.health -= (UnitManager.Instance.SelectedHero.AttackDmg + UnitManager.Instance.SelectedHero.OccupiedTile.DmgBonus);
                            if (enemy.health <= 0)
                            {
                                Destroy(enemy.gameObject);
                                this._enemyHighlight.SetActive(false);
                                clearRangeHighlight();
                            }
                            UnitManager.Instance.SelectedHero.turnTaken = true;
                            ++GameManager.Instance.turnCount;
                            UnitManager.Instance.SetSelectedHero(null);
                            clearRangeHighlight();
                            results = winCheck();
                            if (results == 1)
                            {
                                GameManager.Instance.ChangeState(GameState.HeroesWin);
                                return;
                            }
                            else if (results == 2)
                            {
                                GameManager.Instance.ChangeState(GameState.EnemiesWin);
                                return;
                            }
                            EndTurn(GameManager.Instance.turnCount);
                        }
                    }
                }
            }
            else
            {
                if (UnitManager.Instance.SelectedHero != null)
                {
                    if (isWithinRange(UnitManager.Instance.SelectedHero, this))
                    {
                        resetFactionHighlight(Faction.Hero, UnitManager.Instance.SelectedHero.OccupiedTile, this);
                        SetUnit(UnitManager.Instance.SelectedHero);
                        UnitManager.Instance.SelectedHero.turnTaken = true;
                        ++GameManager.Instance.turnCount;
                        UnitManager.Instance.SetSelectedHero(null);
                        clearRangeHighlight();
                        results = winCheck();
                        if (results == 1)
                        {
                            GameManager.Instance.ChangeState(GameState.HeroesWin);
                            return;
                        }
                        else if (results == 2)
                        {
                            GameManager.Instance.ChangeState(GameState.EnemiesWin);
                            return;
                        }
                        EndTurn(GameManager.Instance.turnCount);
                    }
                }
            }
        }
        else if (GameManager.Instance.GameState == GameState.EnemiesTurn)
        {

            if (OccupiedUnit != null)
            {
                if (OccupiedUnit.Faction == Faction.Enemy && OccupiedUnit.turnTaken == false)
                {
                    UnitManager.Instance.SetSelectedEnemy((BaseEnemy)OccupiedUnit);
                    highlightRange(OccupiedUnit);
                }
                else
                {
                    if (UnitManager.Instance.SelectedEnemy != null)
                    {
                        if (isWithinRange(UnitManager.Instance.SelectedEnemy, this))
                        {
                            var hero = (BaseHero)OccupiedUnit;
                            hero.health -= (UnitManager.Instance.SelectedEnemy.AttackDmg + UnitManager.Instance.SelectedEnemy.OccupiedTile.DmgBonus);
                            if (hero.health <= 0)
                            {
                                Destroy(hero.gameObject);
                                this._heroHighlight.SetActive(false);
                                clearRangeHighlight();
                            }
                            UnitManager.Instance.SelectedEnemy.turnTaken = true;
                            ++GameManager.Instance.turnCount;
                            UnitManager.Instance.SetSelectedEnemy(null);
                            clearRangeHighlight();
                            results = winCheck();
                            if (results == 1)
                            {
                                GameManager.Instance.ChangeState(GameState.HeroesWin);
                                return;
                            }
                            else if (results == 2)
                            {
                                GameManager.Instance.ChangeState(GameState.EnemiesWin);
                                return;
                            }
                            EndTurn(GameManager.Instance.turnCount);
                        }
                    }
                }
            }
            else
            {
                if (UnitManager.Instance.SelectedEnemy != null)
                {
                    if (isWithinRange(UnitManager.Instance.SelectedEnemy, this))
                    {
                        resetFactionHighlight(Faction.Enemy, UnitManager.Instance.SelectedEnemy.OccupiedTile, this);
                        SetUnit(UnitManager.Instance.SelectedEnemy);
                        UnitManager.Instance.SelectedEnemy.turnTaken = true;
                        ++GameManager.Instance.turnCount;
                        UnitManager.Instance.SetSelectedEnemy(null);
                        clearRangeHighlight();
                        results = winCheck();
                        if (results == 1)
                        {
                            GameManager.Instance.ChangeState(GameState.HeroesWin);
                            return;
                        }
                        else if (results == 2)
                        {
                            GameManager.Instance.ChangeState(GameState.EnemiesWin);
                            return;
                        }
                        EndTurn(GameManager.Instance.turnCount);
                    }
                }
            }
        }
    }
    public void SetUnit(BaseUnit unit)
    {
        if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    public void EndTurn(int turnCount)
    {
        if (GameManager.Instance.GameState == GameState.HeroesTurn)
        {
            var remaining = 0;
            var remainingUnits = GameObject.FindObjectsOfType<BaseUnit>();
            foreach (var unit in remainingUnits)
            {
                if (unit.Faction == Faction.Hero)
                {
                    ++remaining;
                }
            }
            if (turnCount == remaining)
            {
                GameManager.Instance.turnCount = 0;
                var allUnits = GameObject.FindObjectsOfType<BaseUnit>();
                foreach (var unit in allUnits)
                {
                    unit.turnTaken = false;
                }
                GameManager.Instance.ChangeState(GameState.EnemiesTurn);
            }
        }
        else if (GameManager.Instance.GameState == GameState.EnemiesTurn)
        {
            var remaining = 0;
            var remainingUnits = GameObject.FindObjectsOfType<BaseUnit>();
            foreach (var unit in remainingUnits)
            {
                if (unit.Faction == Faction.Enemy)
                {
                    ++remaining;
                }
            }
            if (turnCount == remaining)
            {
                GameManager.Instance.turnCount = 0;
                var allUnits = GameObject.FindObjectsOfType<BaseUnit>();
                foreach (var unit in allUnits)
                {
                    unit.turnTaken = false;
                }
                GameManager.Instance.ChangeState(GameState.HeroesTurn);
            }
        }
    }

    public bool isWithinRange(BaseUnit unit, Tile tile)
    {
        var xDiff = Math.Abs(tile.transform.position.x - unit.transform.position.x);

        if (xDiff <= unit.MoveSpeed)
        {
            var yBound = unit.MoveSpeed - xDiff;
            var yDiff = Math.Abs(tile.transform.position.y - unit.transform.position.y);

            if (yDiff <= yBound)
            {
                return true;
            }
        }
        return false;
    }

    public void highlightRange(BaseUnit unit)
    {
        var allTiles = GameObject.FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            if (isWithinRange(unit, tile))
            {
                tile._rangeHighlight.SetActive(true);
            }
            else
            {
                tile._rangeHighlight.SetActive(false);
            }
        }
    }

    public void clearRangeHighlight()
    {
        var allTiles = GameObject.FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            tile._rangeHighlight.SetActive(false);
        }
    }

    public void resetFactionHighlight(Faction faction, Tile oldTile, Tile newTile)
    {
        if (faction == Faction.Hero)
        {
            oldTile._heroHighlight.SetActive(false);
            newTile._heroHighlight.SetActive(true);
        }
        else
        {
            oldTile._enemyHighlight.SetActive(false);
            newTile._enemyHighlight.SetActive(true);
        }
    }

    public int winCheck()
    {
        var allTiles = GameObject.FindObjectsOfType<Tile>();
        var total = 0;
        var heroCount = 0;
        var enemyCount = 0;

        foreach (var tile in allTiles)
        {
            if (tile.OccupiedUnit)
            {
                ++total;
                var unit = tile.OccupiedUnit;

                if (unit.Faction == Faction.Hero)
                {
                    ++heroCount;
                }
                else if (unit.Faction == Faction.Enemy)
                {
                    ++enemyCount;
                }
            }
        }

        GameManager.Instance.charactersRemaining = total;
        GameManager.Instance.heroes = heroCount;
        GameManager.Instance.enemies = enemyCount;

        if (enemyCount == 0)
        {
            return 1;
        }
        else if (heroCount == 0)
        {
            return 2;
        }
        else
        {
            return -1;
        }
    }
}