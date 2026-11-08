<h1>csharp-checkers-winforms</h1>
<p>Desktop implementation of the classic 8x8 Checkers (Draughts) game built with C#, .NET, and Windows Forms</p>

<h2>Project description</h2>
<p>This project implements a fully interactive two-player Checkers game featuring grid generation, custom piece rendering, and standard movement validation rules:</p>
<ol>
  <li>
    <dl>
      <dt><b>Board &amp; State Management:</b></dt>
      <dd>Maintains an 8&times;8 matrix of <code>CheckerType</code> states (<code>White</code>, <code>Black</code>, <code>WhiteKing</code>, <code>BlackKing</code>, and <code>None</code>).</dd>
      <dd>Dynamically constructs an alternating 64-button grid bound to discrete <code>BoardPosition</code> coordinates.</dd>
    </dl>
  </li>
  <li>
    <dl>
      <dt><b>Move &amp; Capture Validation:</b></dt>
      <dd><b>Regular Checkers:</b> Restricts normal moves to single-diagonal forward steps while validating jumping capture moves over opponent pieces.</dd>
      <dd><b>King Promotion:</b> Automatically upgrades regular checkers reaching the opposite baseline (row 7 for White, row 0 for Black) to Kings.</dd>
      <dd><b>Flying Kings:</b> Allows multi-directional diagonal traversals and captures across free board cells.</dd>
    </dl>
  </li>
  <li>
    <dl>
      <dt><b>Interactive Windows Forms UI:</b></dt>
      <dd>Highlights active selected pieces in real time.</dd>
      <dd>Tracks game score counters (captured checkers) and displays the active player turn.</dd>
      <dd>Automates win-state evaluation when an opponent has no remaining active checkers on the board.</dd>
    </dl>
  </li>
</ol>

<h2>Project files</h2>
<table>
  <tr>
    <th colspan="2">csharp-checkers-winforms</th>
  </tr>
  <tr>
    <td>src/Program.cs</td>
    <td>Application root entry point initializing Visual Styles and running Form1</td>
  </tr>
  <tr>
    <td>src/Form1.cs</td>
    <td>Main event controller managing player switching, move validation, and capture mechanics</td>
  </tr>
  <tr>
    <td>src/Form1.Designer.cs</td>
    <td>Windows Forms designer configuration for window layout and score labels</td>
  </tr>
  <tr>
    <td>src/GameBoard.cs</td>
    <td>Board state matrix, visual button grid generator, asset binding, and victory checker</td>
  </tr>
  <tr>
    <td>src/BoardPosition.cs</td>
    <td>Lightweight encapsulation of zero-indexed (Row, Column) grid coordinates</td>
  </tr>
  <tr>
    <td>Assets/</td>
    <td>Sprite assets for normal and crowned checkers (black/white pieces)</td>
  </tr>
  <tr>
    <td>README.md</td>
    <td>Project documentation and architectural overview</td>
  </tr>
  <tr>
    <td>.gitignore</td>
    <td>Exclusion rules for compiled binaries (.exe, .dll), obj directories, and IDE user states</td>
  </tr>
</table>

<h2>Rules &amp; Mechanics Implemented</h2>
<table>
  <tr>
    <th>Feature</th>
    <th>Implementation Detail</th>
  </tr>
  <tr>
    <td><b>Grid Dimensions</b></td>
    <td>8 &times; 8 alternating cells (400 &times; 400 px playing area)</td>
  </tr>
  <tr>
    <td><b>Initial Setup</b></td>
    <td>12 white pieces (rows 0–2) vs. 12 black pieces (rows 5–7) on dark squares</td>
  </tr>
  <tr>
    <td><b>Player Turn Flow</b></td>
    <td>White initiates (Player 1), followed by automatic turn switching upon valid moves</td>
  </tr>
  <tr>
    <td><b>Selection UI</b></td>
    <td>Selected piece highlighted in yellow; invalid moves blocked with user notifications</td>
  </tr>
  <tr>
    <td><b>Victory Condition</b></td>
    <td>Game terminates with message dialog when all opponent pieces are eliminated</td>
  </tr>
</table>

<h2>Installation and launch</h2>
<ol>
  <b>
    <li>
      <dl>
        <dt>Clone repository</dt>
        <dd>
          <code>git clone https://github.com/Kyoryuu-lab/csharp-checkers-winforms.git</code>
        </dd>
        <dd>
          <code>cd csharp-checkers-winforms</code>
        </dd>
      </dl>
    </li>
  </b>
  <b>
    <li>
      <dl>
        <dt>Build and run via Visual Studio / MSBuild</dt>
        <dd>Open <code>checkers.sln</code> in Visual Studio or JetBrains Rider and press <b>F5 (Start Debugging)</b>.</dd>
        <dd>Or build from CLI via MSBuild / .NET CLI:</dd>
        <dd>
          <code>msbuild checkers.sln /p:Configuration=Release</code>
        </dd>
      </dl>
    </li>
  </b>
</ol>

<h2>Example of gameplay UI</h2>
<samp>
  <p>
    ========================================================================<br />
    &nbsp;&nbsp;CHECKERS (DRAUGHTS) DESKTOP APPLICATION<br />
    ========================================================================<br />
    [*] Game initialized: 8x8 Board populated.<br />
    [*] Player 1 (White) active.<br />
    <br />
    --- Status Display ---<br />
    Current Turn : Player 1 (White)<br />
    Score&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;: Player 1: 3 | Player 2: 1<br />
    Selected Cell: [Row 2, Col 3] &rarr; Highlighted [Yellow]<br />
    Target Cell&nbsp;&nbsp;: [Row 4, Col 5] &rarr; Valid Capture Move [Opponent Piece Removed]<br />
    King Event&nbsp;&nbsp;&nbsp;: Piece promoted to WhiteKing at [Row 7, Col 1]<br />
    ========================================================================
  </p>