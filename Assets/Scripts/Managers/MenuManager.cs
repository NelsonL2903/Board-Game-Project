using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] private GameObject _selectedHeroObject, _selectedEnemyObject, _tileObject, _tileUnitObject, _playerTurn;

    void Awake()
    {
        Instance = this;
    }

    public void ShowTileInfo(Tile tile)
    {

        if (tile == null)
        {
            _tileObject.SetActive(false);
            _tileUnitObject.SetActive(false);
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
        _tileObject.SetActive(true);

        if (tile.OccupiedUnit)
        {
            _tileUnitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName + " " + tile.OccupiedUnit.health + " HP";
            _tileUnitObject.SetActive(true);
        }
    }

    public void ShowSelectedHero(BaseHero hero)
    {
        if (hero == null)
        {
            _selectedHeroObject.SetActive(false);
            return;
        }

        _selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName + " " + hero.health + " HP";
        _selectedHeroObject.SetActive(true);
    }

    public void ShowSelectedEnemy(BaseEnemy enemy)
    {
        if (enemy == null)
        {
            _selectedEnemyObject.SetActive(false);
            return;
        }

        _selectedEnemyObject.GetComponentInChildren<Text>().text = enemy.UnitName + " " + enemy.health + " HP";
        _selectedEnemyObject.SetActive(true);
    }

    public void showPlayerTurn(GameState state)
    {
        if (state == GameState.HeroesTurn)
        {
            _playerTurn.GetComponentInChildren<Text>().text = "Player 1's turn (BLUE)";
            _playerTurn.SetActive(true);
            return;
        }
        else if (state == GameState.EnemiesTurn)
        {
            _playerTurn.GetComponentInChildren<Text>().text = "Player 2's turn (RED)";
            _playerTurn.SetActive(true);
            return;
        }
        else
        {
            _playerTurn.SetActive(false);
        }
    }
}