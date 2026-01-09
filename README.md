# RobotWorld

> Projet étudiant Unity (Module R5.A.06) - Jeu de plateforme 3D.

![Unity](https://img.shields.io/badge/Unity-2021.3%2B-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

## À propos

**RobotWorld** est un jeu de plateforme en 3D où vous incarnez **Rob**, un petit robot agile. Votre mission est d'explorer un monde ouvert, d'éliminer des ennemis et de résoudre des énigmes environnementales pour libérer une orbe sacrée.

[cite_start]L'objectif principal est de **retrouver 4 orbes d'argent** dispersées dans la carte pour détruire la cage qui protège l'orbe finale[cite: 11, 12, 28].

## Contrôles

| Action | Touche / Commande | Description |
| :--- | :--- | :--- |
| **Déplacement** | `W`, `A`, `S`, `D` | [cite_start]Se déplacer dans la scène [cite: 14] |
| **Caméra** | `Clic Droit` + `Souris` | [cite_start]Caméra orbitale autour du robot [cite: 15] |
| **Saut** | `Espace` | Saut classique. [cite_start]Permet d'éliminer les ennemis en leur sautant dessus [cite: 17, 18] |
| **Double Saut** | `Espace` (x2) | [cite_start]Pour atteindre les plateformes élevées [cite: 19, 21] |
| **Sprint** | `Shift` (Maintenir) | [cite_start]Courir plus vite (limité par la barre d'endurance) [cite: 24, 26] |

## Objectifs & Astuces

Pour gagner, vous devez trouver **4 Orbes d'Argent** :

1.  Gardée par un ennemi près de l'arbre central.
2.  Cachée derrière les rochers près des montagnes.
3.  Située derrière la colline.
4.  Verrouillée dans un coffre. Vous devrez d'abord trouver la **clé** (Indice : cherchez près des structures en pierre).

## Installation & Lancement

1.  Clonez ce dépôt :
    ```bash
    git clone [https://github.com/votre-username/RobotWorld.git](https://github.com/votre-username/RobotWorld.git)
    ```
2.  Ouvrez le projet avec **Unity Hub** (Version recommandée : 2022.x ou supérieure).
3.  Ouvrez la scène principale dans `Assets/Scenes`.
4.  Appuyez sur le bouton **Play**.

## Auteurs & Rôles

Ce projet a été réalisé en binôme dans le cadre du module R5A06[cite: 2]:

* **Nassim BUCHMULLER** : Physique/mécaniques du jeu, UI & personnalisation, Décors, Level Design, Scripts.
* **Matteo BELZ** : Scripts, IA & Physique des ennemis, Mécaniques de jeu, Obstacles.

## Crédits & Assets

Nous remercions les créateurs des assets 3D utilisés dans ce projet :

* **Personnage :** *Animated Robot* par Quaternius (Poly Pizza)[cite: 36].
* **Ennemis :** *Low Poly Robot Scorpion* par oconop23[cite: 45].
* **Environnement :**
    * *Free 3D 6 Trees Collection* (TurboSquid)[cite: 41].
    * *Treasure Chest* par Hidden Ghillie Dhu[cite: 43].
    * *Rust Key* (Unity Asset Store) - Modifié pour le jeu[cite: 37, 39].

---
*Projet réalisé pour l'IUT d'Aix-Marseille*
